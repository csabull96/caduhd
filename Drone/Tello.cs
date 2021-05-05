using Caduhd.Common;
using Caduhd.Controller;
using Caduhd.Controller.Command;
using Caduhd.Drone.Event;
using Emgu.CV;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    public class Tello : IControllableDrone
    {
        private const string CONNECT = "command";
        private const string TAKE_OFF = "takeoff";
        private const string LAND = "land";
        private const string START_STREAMING_VIDEO = "streamon";
        private const string STOP_STREAMING_VIDEO = "streamoff";
        private const string MOVE = "rc {0} {1} {2} {3}";
        private const string QUERY_WIFI_SIGNAL_QUALITY = "wifi?";

        private UdpClient _udpServer = new UdpClient(new IPEndPoint(IPAddress.Parse("0.0.0.0"), 8890));
        private UdpClient _udpClient = new UdpClient(11001);
        private IPEndPoint _telloIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.10.1"), 8889);
        private byte[] _messageBuffer = default;
        private byte[] _telloStateData = default;

        private const int MIN_SPEED = 0;
        private const int MAX_SPEED = 100;
        private const int DEFAULT_SPEED = 100;
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

        private CancellationTokenSource _videoStreamCancellationTokenSource;

        private DroneState telloState = new DroneState();

        private bool _isStreamingVideo = false;
        public bool IsStreamingVideo => _isStreamingVideo;

        public Tello() { }
    
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
                    // not handled yet
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

        private void StartDroneStateReceiver()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    _telloStateData = _udpServer.Receive(ref _telloIPEndPoint);
                    try
                    {
                        var properties = Encoding.ASCII.GetString(_telloStateData).Split(';');

                        telloState.Pitch = int.Parse(properties[0].Substring(6));
                        telloState.Roll = int.Parse(properties[1].Substring(5));
                        telloState.Yaw = int.Parse(properties[2].Substring(4));
                        telloState.SpeedX = int.Parse(properties[3].Substring(4));
                        telloState.SpeedY = int.Parse(properties[4].Substring(4));
                        telloState.SpeedZ = int.Parse(properties[5].Substring(4));
                        telloState.LowestTemperature = int.Parse(properties[6].Substring(6));
                        telloState.HighestTemperature = int.Parse(properties[7].Substring(6));
                        telloState.TOF = int.Parse(properties[8].Substring(4));
                        telloState.Height = int.Parse(properties[9].Substring(2));
                        telloState.Battery = int.Parse(properties[10].Substring(4));
                        telloState.Barometer = double.Parse(properties[11].Substring(5));
                        telloState.Time = int.Parse(properties[12].Substring(5));
                        telloState.AccelerationX = double.Parse(properties[13].Substring(4));
                        telloState.AccelerationY = double.Parse(properties[14].Substring(4));
                        telloState.AccelerationZ = double.Parse(properties[15].Substring(4));

                        StateChanged?.Invoke(this, new DroneStateChangedEventArgs(telloState));
                    }
                    catch
                    {
                        // todo: 
                    }
                   
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void Connect()
        {
            TellTelloTo(CONNECT);
            // we should only continue if there was an okay response
            StartDroneStateReceiver();
        }

        private void Disconnect()
        {
            throw new NotImplementedException();
        }

        private void TakeOff()
        {
            TellTelloTo(TAKE_OFF);           
        }

        private void Land()
        {
            TellTelloTo(LAND);
        }

        private void Move(MoveCommand moveCommand)
        {
            TellTelloTo(string.Format(MOVE, 
                moveCommand.Lateral * Speed,
                moveCommand.Longitudinal * Speed,
                moveCommand.Vertical * Speed,
                moveCommand.Yaw * Speed));
        }

        private void StartStreamingVideo()
        {
            // i should probably always check if the connection is still alive
            if (!IsStreamingVideo)
            {
                TellTelloTo(START_STREAMING_VIDEO);

                _videoStreamCancellationTokenSource = new CancellationTokenSource();

                Task.Factory.StartNew(() =>
                {
                    _isStreamingVideo = true;
                    VideoCapture videoCapture = new VideoCapture("udp://0.0.0.0:11111");
                    while (IsStreamingVideo)
                    {
                        if (_videoStreamCancellationTokenSource.IsCancellationRequested)
                        {
                            videoCapture.Dispose();
                            _isStreamingVideo = false;
                        }
                        else
                        {
                            // var frameCount = vc.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                            Mat frame = new Mat();
                            videoCapture.Read(frame);
                            BgrImage image = new BgrImage(frame);
                            NewCameraFrame?.Invoke(this, new NewDroneCameraFrameEventArgs(image));
                        }
                    }
                }, _videoStreamCancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        private void StopStreamingVideo()
        {
            if (IsStreamingVideo)
            {
                _isStreamingVideo = false;

                _videoStreamCancellationTokenSource.Cancel();
                _videoStreamCancellationTokenSource.Dispose();

                TellTelloTo(STOP_STREAMING_VIDEO);
            }
        }

        private void TellTelloTo(string command)
        {
            _messageBuffer = Encoding.ASCII.GetBytes(command);
            _udpClient.Send(_messageBuffer, _messageBuffer.Length, _telloIPEndPoint);

            // throws error if wifi not conencted
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
