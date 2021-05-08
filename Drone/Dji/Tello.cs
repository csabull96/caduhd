using Caduhd.Common;
using Caduhd.Controller;
using Caduhd.Controller.Command;
using Caduhd.Drone.Event;
using Emgu.CV;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Caduhd.Drone.Dji
{
    public class Tello : IControllableDrone, IDisposable
    {
        private bool _disposing;

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

        // what connected really means is that there was a handshake after the initial connect message
        private bool _connected;

        private ConcurrentQueue<string> _commandQueue;

        private const string TELLO_IP = "192.168.10.1";
        private const int TELLO_PORT = 8889;
        private IPEndPoint _telloIPEndPoint;
        // to send messages to Tello and receive their response
        private readonly UdpClient _tello;
        private int _isReceivingTelloResponse;
        // to receive Tello's state
        private readonly UdpClient _telloStateReceiver;
        private int _isReceivingTelloState;
        private TelloStateParser _telloStateParser;
        // the connection with Tello is UDP based so
        // instead of "connected" vs "not connected"
        // I went with "responding" vs "not responding"
        public bool IsReachable { get; private set; }

        private DroneState _droneState;
        private readonly Timer _snrChecker;
        private int _snr;

        private const int MIN_SPEED = 0;
        private const int MAX_SPEED = 100;
        private const int DEFAULT_SPEED = 60;
        private int _speed = DEFAULT_SPEED;
        public int Speed 
        {
            get => _speed;
            set => _speed = AdjustSpeed(value);
        }

        public delegate void NewDroneVideoFrameEventHandler(object source, NewDroneCameraFrameEventArgs args);
        public event NewDroneVideoFrameEventHandler NewCameraFrame;

        public delegate void DroneStateEventHandler(object source, DroneStateChangedEventArgs args);
        public event DroneStateEventHandler StateChanged;

        private int _isStreaming;
        private CancellationTokenSource _videoStreamCancellationTokenSource;
        private int _numberOfConsecutiveEmtpyFrames;
        private const int ALLOWED_NUMBER_OF_CONSECUTIVE_EMPTY_FRAMES = 10;

        public bool IsStreamingVideo { get; private set; } = false;

        public Tello() 
        {
            _disposing = false;

            _telloIPEndPoint = new IPEndPoint(IPAddress.Parse(TELLO_IP), TELLO_PORT);
            _tello = new UdpClient(11001);
            _tello.Client.ReceiveTimeout = TELLO_RESPONSE_TIMEOUT;
            _isReceivingTelloResponse = NO;

            _telloStateReceiver = new UdpClient(new IPEndPoint(IPAddress.Parse("0.0.0.0"), 8890));
            _telloStateReceiver.Client.ReceiveTimeout = TELLO_STATE_RECEIVER_RESPONSE_TIMEOUT;
            _isReceivingTelloState = NO;
            _telloStateParser = new TelloStateParser();

            _connected = false;

            _commandQueue = new ConcurrentQueue<string>();

            _snr = -1;
            _snrChecker = new Timer(WIFI_CHECKER_TIME_INTERVAL);
            _snrChecker.Elapsed += Elapsed;

            _isStreaming = 0;
            _videoStreamCancellationTokenSource = new CancellationTokenSource();
            _numberOfConsecutiveEmtpyFrames = 0;
            
            StartResponseReceiver();
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_disposing)
            {
                _snrChecker.Dispose();
            }
            else if (!_connected)
            {
                _snrChecker.Stop();
            }
            else if (IsReachable)
            {
                TellTello(REQUEST_WIFI_SNR);
            }
        }

        public void Control(DroneCommand droneCommand)
        {
            if (droneCommand is MovementCommand)
            {
                if (droneCommand is TakeOffCommand)
                {
                    TakeOff();
                }
                else if (droneCommand is LandCommand)
                {
                   Land();
                }
                else if (droneCommand is MoveCommand)
                {
                    Move(droneCommand as MoveCommand);
                }
            }
            else if (droneCommand is ControlCommand)
            {
                if (droneCommand is ConnectCommand)
                {
                    Connect();
                }
                else if (droneCommand is DisconnectCommand)
                {
                    // the connection is UDP based
                }
            }
            else if (droneCommand is CameraCommand)
            {
                if (droneCommand is StartStreamingVideoCommand)
                {
                    StartStreamingVideo();
                }
                else if (droneCommand is StopStreamingVideoCommand)
                {
                    StopStreamingVideo();
                }
            }
        }

        private void Connect()
        {
            // if the Tello successfully received our connect message
            // then it'll cache the senders IP/Port
            // every message we send to the Tello (even from different socket)
            // the Tello is going to answer to the originally cached address
            // until it receives a connect message with a different IP/Port
            TellTello(CONNECT);            
        }

        private void Disconnect()
        {
            _connected = false;
            // should we call dispose here??
        }

        private void TakeOff()
        {
            // we can't take off unless our initial connect message has been answered
            // land and move methods have no such restrictions (safety reasons)
            if (_connected)
            {
                TellTello(TAKE_OFF);
            }
        }

        private void Land()
        {
            TellTello(LAND);
        }

        private void Move(MoveCommand moveCommand)
        {
            TellTello(string.Format(MOVE,
                moveCommand.Lateral * Speed,
                moveCommand.Longitudinal * Speed,
                moveCommand.Vertical * Speed,
                moveCommand.Yaw * Speed));
        }

        private void StartStreamingVideo()
        {
            if (!_connected || Interlocked.CompareExchange(ref _isStreaming, YES, NO) == YES)
                return;
            
            TellTello(START_STREAMING_VIDEO);

            _videoStreamCancellationTokenSource = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                using (VideoCapture videoCapture = new VideoCapture("udp://0.0.0.0:11111"))
                {
                    if (videoCapture.IsOpened && videoCapture.BackendName == "FFMPEG")
                    {
                        IsStreamingVideo = true;

                        while (!_videoStreamCancellationTokenSource.IsCancellationRequested && !_disposing && _connected)
                        {
                            Mat frame = new Mat();
                            videoCapture.Read(frame);
                            
                            if (frame.IsEmpty)
                            {
                                if (ALLOWED_NUMBER_OF_CONSECUTIVE_EMPTY_FRAMES < ++_numberOfConsecutiveEmtpyFrames)
                                {
                                    _videoStreamCancellationTokenSource.Cancel();
                                }

                                continue;
                            }

                            _numberOfConsecutiveEmtpyFrames = 0;
                            BgrImage image = new BgrImage(frame);
                            frame.Dispose();
                            NewCameraFrame?.Invoke(this, new NewDroneCameraFrameEventArgs(image));
                        }

                        TellTello(STOP_STREAMING_VIDEO);
                        IsStreamingVideo = false;
                        NewCameraFrame?.Invoke(this, new NewDroneCameraFrameEventArgs(BgrImage.GetBlank(Color.Orange)));
                    }
                }

                Interlocked.Exchange(ref _isStreaming, NO);

            }, _videoStreamCancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        }

        private void StopStreamingVideo()
        {
            if (IsStreamingVideo)
            {
                _videoStreamCancellationTokenSource.Cancel();
            }
        }

        private void TellTello(string commandString)
        {
            try
            {
                _commandQueue.Enqueue(commandString);
                byte[] commandBytes = commandString.AsBytes();
                IPEndPoint telloIPEndPoint = new IPEndPoint(IPAddress.Parse(TELLO_IP), TELLO_PORT);
                _tello.Send(commandBytes, commandBytes.Length, telloIPEndPoint);
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
            if (Interlocked.CompareExchange(ref _isReceivingTelloResponse, YES, NO) == YES)
                return;

            await Task.Factory.StartNew(() =>
            {
                while (!_disposing)
                {
                    if (_commandQueue.TryDequeue(out string commandString))
                    {
                        string responseString = string.Empty;
                        try
                        {
                            byte[] responseBytes = _tello.Receive(ref _telloIPEndPoint);
                            IsReachable = true;
                            responseString = responseBytes.AsString();
                            // response is only forwarded to the ProcessResponse method
                            // if it comes from the Tello
                            ProcessResponse(commandString, responseString);
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

                // we only get here if _disposing = true
                _tello.Close();

            }, TaskCreationOptions.LongRunning);
        }

        private void ProcessResponse(string command, string response)
        {
            if (command == CONNECT)
            {
                // if we receive any kind of response
                // (other than "error") right after the "command" message was sent
                // we take it as a "successful handshake"
                if (!_connected && response != ERROR_RESPONSE)
                {
                    _connected = true;
                    StartReceivingTelloState();
                    _snrChecker.Start();
                }
            }
            else if (command == REQUEST_WIFI_SNR)
            {
                if (int.TryParse(response, out int snr))
                {
                    _snr = snr;
                }
            }
        }

        private async void StartReceivingTelloState()
        {
            if (!_connected || Interlocked.CompareExchange(ref _isReceivingTelloState, YES, NO) == YES)
                return;

            await Task.Factory.StartNew(() =>
            {
                while (!_disposing && _connected)
                {
                    try
                    {
                        byte[] stateBytes = _telloStateReceiver.Receive(ref _telloIPEndPoint);
                        IsReachable = true;
                        _droneState = _telloStateParser.Parse(stateBytes);
                        _droneState.Wifi = _snr;

                        StateChanged?.Invoke(this, new DroneStateChangedEventArgs(_droneState));
                        
                        // should we restart video streaming and wifi checker if Tello is reachable again?
                    }
                    catch (SocketException e)
                    {
                        if (e.SocketErrorCode == SocketError.TimedOut)
                        {
                            // Tello stopped sharing its state, probably Tello is not reachable
                            IsReachable = false;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }

                if (_disposing)
                {
                    _telloStateReceiver.Close();
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

        public void Dispose()
        {
            _disposing = true;
        }
    }
}
