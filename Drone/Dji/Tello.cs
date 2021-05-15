namespace Caduhd.Drone.Dji
{
    using System;
    using System.Collections.Concurrent;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;
    using Caduhd.Common;
    using Caduhd.Drone.Command;
    using Caduhd.Drone.Event;
    using Emgu.CV;
    using Timer = System.Timers.Timer;

    /// <summary>
    /// The implementation of the <see cref="AbstractDrone"/> for the DJI Tello.
    /// </summary>
    public class Tello : AbstractDrone, IStreamer, IStateful
    {
        private const int YES = 1;
        private const int NO = 0;

        private const int TELLO_RESPONSE_TIMEOUT = 200;
        private const int TELLO_STATE_RECEIVER_RESPONSE_TIMEOUT = 2000;
        private const int WIFI_CHECKER_TIME_INTERVAL = 1600;

        private const string OK_RESPONSE = "ok";
        private const string ERROR_RESPONSE = "error";

        private const string CONNECT = "command";
        private const string TAKE_OFF = "takeoff";
        private const string LAND = "land";
        private const string MOVE = "rc {0} {1} {2} {3}";
        private const string START_STREAMING_VIDEO = "streamon";
        private const string STOP_STREAMING_VIDEO = "streamoff";
        private const string REQUEST_WIFI_SNR = "wifi?";

        private const string TELLO_IP = "192.168.10.1";
        private const int TELLO_PORT = 8889;

        private const int MIN_SPEED = 0;
        private const int MAX_SPEED = 100;
        private const int DEFAULT_SPEED = 60;

        private const int ALLOWED_NUMBER_OF_CONSECUTIVE_EMPTY_FRAMES = 10;

        private readonly UdpClient tello;
        private readonly UdpClient telloStateReceiver;

        private readonly Timer snrChecker;

        private bool disposing;

        // what connected really means is that there was a handshake after the initial connect message
        private bool connected;

        private ConcurrentQueue<string> commandQueue;

        private IPEndPoint telloIPEndPoint;

        // to send messages to Tello and receive their response
        private int isReceivingTelloResponse;

        // to receive Tello's state
        private int isReceivingTelloState;
        private TelloStateParser telloStateParser;

        private DroneState droneState;
        private int snr;

        private int speed = DEFAULT_SPEED;

        private int isStreaming;
        private int numberOfConsecutiveEmtpyFrames;
        private CancellationTokenSource videoStreamCancellationTokenSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tello"/> class.
        /// </summary>
        public Tello()
            : this(TELLO_IP, TELLO_PORT)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tello"/> class.
        /// </summary>
        /// <param name="ip">The IP addres of the Tello.</param>
        /// <param name="port">The port number.</param>
        public Tello(string ip, int port)
        {
            this.disposing = false;

            this.telloIPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            this.tello = new UdpClient(11001);
            this.tello.Client.ReceiveTimeout = TELLO_RESPONSE_TIMEOUT;
            this.isReceivingTelloResponse = NO;

            this.telloStateReceiver = new UdpClient(new IPEndPoint(IPAddress.Parse("0.0.0.0"), 8890));
            this.telloStateReceiver.Client.ReceiveTimeout = TELLO_STATE_RECEIVER_RESPONSE_TIMEOUT;
            this.isReceivingTelloState = NO;
            this.telloStateParser = new TelloStateParser();

            this.connected = false;

            this.commandQueue = new ConcurrentQueue<string>();

            this.snr = -1;
            this.snrChecker = new Timer(WIFI_CHECKER_TIME_INTERVAL);
            this.snrChecker.Elapsed += this.Elapsed;

            this.isStreaming = 0;
            this.videoStreamCancellationTokenSource = new CancellationTokenSource();
            this.numberOfConsecutiveEmtpyFrames = 0;
        }

        /// <summary>
        /// This event fires whenever there is a new available drone camera frame.
        /// </summary>
        public event NewDroneVideoFrameEventHandler NewCameraFrame;

        /// <summary>
        /// This event fires whenever the Tello's state has been updated.
        /// </summary>
        public event DroneStateChangedEventHandler StateChanged;

        // the connection with Tello is UDP based so
        // instead of "connected" vs "not connected"
        // I went with "responding" vs "not responding"

        /// <summary>
        /// Gets a value indicating whether the Telli is reachable or not.
        /// </summary>
        public bool IsReachable { get; private set; }

        /// <summary>
        /// Gets or sets the speed of the Tello. Maximum 100, minimum 0. (Not in real-life speed unit.)
        /// </summary>
        public int Speed
        {
            get => this.speed;
            set => this.speed = this.AdjustSpeed(value);
        }

        /// <summary>
        /// Gets a value indicating whether the Tello is streaming video at the moment or not.
        /// </summary>
        public bool IsStreamingVideo { get; private set; } = false;

        /// <summary>
        /// Disposes Tello.
        /// </summary>
        public override void Dispose()
        {
            this.disposing = true;
            this.telloStateReceiver.Close();
            this.tello.Close();
        }

        /// <summary>
        /// Sends the requested command for execution to the drone.
        /// </summary>
        /// <param name="droneCommand">The requested <see cref="DroneCommand"/> object.</param>
        public override void Control(DroneCommand droneCommand)
        {
            if (droneCommand is MovementCommand)
            {
                if (droneCommand is TakeOffCommand)
                {
                    this.TakeOff();
                }
                else if (droneCommand is LandCommand)
                {
                    this.Land();
                }
                else if (droneCommand is MoveCommand)
                {
                    this.Move(droneCommand as MoveCommand);
                }
            }
            else if (droneCommand is ControlCommand)
            {
                if (droneCommand is ConnectCommand)
                {
                    this.Connect();
                }
                else if (droneCommand is DisconnectCommand)
                {
                    this.Disconnect();
                }
            }
            else if (droneCommand is CameraCommand)
            {
                if (droneCommand is StartStreamingVideoCommand)
                {
                    this.StartStreamingVideo();
                }
                else if (droneCommand is StopStreamingVideoCommand)
                {
                    this.StopStreamingVideo();
                }
            }
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.disposing)
            {
                this.snrChecker.Dispose();
            }
            else if (!this.connected)
            {
                this.snrChecker.Stop();
            }
            else if (this.IsReachable)
            {
                this.TellTello(REQUEST_WIFI_SNR);
            }
        }

        private void Connect()
        {
            this.StartResponseReceiver();

            // if the Tello successfully received our connect message
            // then it'll cache the senders IP/Port
            // every message we send to the Tello (even from different socket)
            // the Tello is going to answer to the originally cached address
            // until it receives a connect message with a different IP/Port
            this.TellTello(CONNECT);
        }

        private void Disconnect()
        {
            this.connected = false;
        }

        private void TakeOff()
        {
            // we can't take off unless our initial connect message has been answered
            // land and move methods have no such restrictions (safety reasons)
            if (this.connected)
            {
                this.TellTello(TAKE_OFF);
            }
        }

        private void Land()
        {
            this.TellTello(LAND);
        }

        private void Move(MoveCommand moveCommand)
        {
            this.TellTello(
                string.Format(
                    MOVE,
                    moveCommand.Lateral * this.Speed,
                    moveCommand.Longitudinal * this.Speed,
                    moveCommand.Vertical * this.Speed,
                    moveCommand.Yaw * this.Speed));
        }

        private void StartStreamingVideo()
        {
            if (!this.connected || Interlocked.CompareExchange(ref this.isStreaming, YES, NO) == YES)
            {
                return;
            }

            this.TellTello(START_STREAMING_VIDEO);

            this.videoStreamCancellationTokenSource = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                using (VideoCapture videoCapture = new VideoCapture("udp://0.0.0.0:11111"))
                {
                    if (videoCapture.IsOpened && videoCapture.BackendName == "FFMPEG")
                    {
                        this.IsStreamingVideo = true;

                        while (!this.videoStreamCancellationTokenSource.IsCancellationRequested && !this.disposing && this.connected)
                        {
                            Mat frame = new Mat();
                            videoCapture.Read(frame);

                            if (frame.IsEmpty)
                            {
                                if (ALLOWED_NUMBER_OF_CONSECUTIVE_EMPTY_FRAMES < ++this.numberOfConsecutiveEmtpyFrames)
                                {
                                    this.videoStreamCancellationTokenSource.Cancel();
                                }

                                continue;
                            }

                            this.numberOfConsecutiveEmtpyFrames = 0;
                            BgrImage image = new BgrImage(frame);
                            frame.Dispose();
                            this.NewCameraFrame?.Invoke(this, new NewDroneCameraFrameEventArgs(image));
                        }

                        this.TellTello(STOP_STREAMING_VIDEO);
                        this.IsStreamingVideo = false;

                        this.NewCameraFrame?.Invoke(this, new NewDroneCameraFrameEventArgs(BgrImage.GetBlank(1920, 1080, Color.WhiteSmoke)));
                    }
                }

                Interlocked.Exchange(ref this.isStreaming, NO);
            }, this.videoStreamCancellationTokenSource.Token,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
        }

        private void StopStreamingVideo()
        {
            if (this.IsStreamingVideo)
            {
                this.videoStreamCancellationTokenSource.Cancel();
            }
        }

        private void TellTello(string commandString)
        {
            try
            {
                this.commandQueue.Enqueue(commandString);
                byte[] commandBytes = commandString.AsBytes();
                this.tello.Send(commandBytes, commandBytes.Length, this.telloIPEndPoint);
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.TimedOut)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        private async void StartResponseReceiver()
        {
            if (Interlocked.CompareExchange(ref this.isReceivingTelloResponse, YES, NO) == YES)
            {
                return;
            }

            await Task.Factory.StartNew(() =>
            {
                while (!this.disposing)
                {
                    if (this.commandQueue.TryDequeue(out string commandString))
                    {
                        string responseString = string.Empty;
                        try
                        {
                            byte[] responseBytes = this.tello.Receive(ref this.telloIPEndPoint);
                            this.IsReachable = true;
                            responseString = responseBytes.AsString().Trim();
                            this.ProcessResponse(commandString, responseString);
                        }
                        catch (SocketException e)
                        {
                            if (e.SocketErrorCode == SocketError.TimedOut)
                            {
                                // do not set IsReachable = false here, because appearently not every message gets responded
                                responseString = "No response.";
                            }
                            else
                            {
                                responseString = $"Receiving has failed, error message: {e.Message}";
                            }
                        }
                        catch (Exception e)
                        {
                            responseString = $"Unexpected exception has occured, error message: {e.Message}";
                        }

                        File.AppendAllText("responses.txt", $"{commandString,-20} >> {responseString}\n");
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void ProcessResponse(string command, string response)
        {
            if (command == CONNECT)
            {
                // if we receive any kind of response
                // (other than "error") right after the "command" message was sent
                // we take it as a "successful handshake"
                if (!this.connected && response != ERROR_RESPONSE)
                {
                    this.connected = true;
                    this.StartReceivingTelloState();
                    this.snrChecker.Start();
                }
            }
            else if (command == REQUEST_WIFI_SNR)
            {
                if (int.TryParse(response, out int snr))
                {
                    this.snr = snr;
                }
            }
        }

        private async void StartReceivingTelloState()
        {
            if (!this.connected || Interlocked.CompareExchange(ref this.isReceivingTelloState, YES, NO) == YES)
            {
                return;
            }

            await Task.Factory.StartNew(() =>
            {
                while (!this.disposing && this.connected)
                {
                    try
                    {
                        byte[] stateBytes = this.telloStateReceiver.Receive(ref this.telloIPEndPoint);
                        this.IsReachable = true;
                        this.droneState = this.telloStateParser.Parse(stateBytes);
                        this.droneState.WiFiSnr = this.snr;

                        this.StateChanged?.Invoke(this, new DroneStateChangedEventArgs(this.droneState));

                        // should we restart video streaming and wifi checker if Tello is reachable again?
                    }
                    catch (SocketException e)
                    {
                        if (e.SocketErrorCode == SocketError.TimedOut)
                        {
                            // Tello stopped sharing its state, probably Tello is not reachable
                            this.IsReachable = false;
                            this.droneState = new DroneState();
                            this.StateChanged?.Invoke(this, new DroneStateChangedEventArgs(this.droneState));
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        private int AdjustSpeed(int original)
        {
            if (original < MIN_SPEED)
            {
                return MIN_SPEED;
            }
            else if (MAX_SPEED < original)
            {
                return MAX_SPEED;
            }

            return original;
        }
    }
}
