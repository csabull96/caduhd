using Caduhd.Controller;
using Caduhd.Controller.Commands;
using Caduhd.Drone;
using Caduhd.HandDetector.Detector;
using Caduhd.HandDetector.Model;
using Caduhd.Input.Camera;
using Caduhd.Input.Keyboard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Caduhd.UserInterface.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public UserInterfaceConnector UserInterfaceConnector { get; private set; } = new UserInterfaceConnector();
        private IWebCamera m_webCamera = new WebCamera(5);
        private AbstractDrone m_drone = new Tello();
        private IHandDetector m_handDetector = new Caduhd.HandDetector.Detector.HandDetector();
        private DroneController m_droneController;

        public MainViewModel()
        {
            m_webCamera.NewFrame += HandleWebCameraFrame;
            m_webCamera.TurnOn();

            m_drone.NewDroneCameraFrame += HandleDroneCameraFrame;
            m_drone.StateChanged += HandleDroneStateChanged;

            m_droneController = new DroneController(m_handDetector);
            m_droneController.InputEvaluated += ControlDrone;
            m_droneController.HandDetectorStateChanged += HandleHandDetectorStateChanged;
            m_droneController.WebCameraFrameProcessed += HandleWebCameraFrameProcessed;

            // only for debugging purposes
            m_droneController.HandsDetected += (s, args) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Hand left = (s as Hands).Left;
                    Hand right = (s as Hands).Right;

                    UserInterfaceConnector.LeftHand = $"{left.Position.X}/{left.Position.Y}/{left.Weight}";
                    UserInterfaceConnector.RightHand = $"{right.Position.X}/{right.Position.Y}/{right.Weight}";
                    UserInterfaceConnector.Direction = $"{args.Movement}";
                });
            };
        }

        private void HandleWebCameraFrameProcessed(object sender, InputProcessedEventArgs eventArgs)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UserInterfaceConnector.CurrentWebCameraFrame = BitmapToBitmapSource(eventArgs.ProcessedFrame, PixelFormats.Bgr24);
            });
        }

        private void HandleWebCameraFrame(object sender, NewWebCameraFrameEventArgs args)
        {
            m_droneController.HandleWebCameraInput(args.Frame);
        }

        private void HandleDroneCameraFrame(object source, NewDroneCameraFrameEventArgs evetArgs)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UserInterfaceConnector.CurrentDroneCameraFrame = BitmapToBitmapSource(evetArgs.Frame, PixelFormats.Bgr24);
            });
        }

        private void HandleDroneStateChanged(object source, DroneStateChangedEventArgs eventArgs)
        {
            UserInterfaceConnector.SetSpeed(eventArgs.DroneState.Speed);
            UserInterfaceConnector.SetHeight(eventArgs.DroneState.Height);
            UserInterfaceConnector.SetBatteryLevel(eventArgs.DroneState.Battery);
        }

        private void ControlDrone(object source, DroneControllerInputEvaluatedEventArgs eventArgs)
        {
            if (eventArgs.DroneCommand is DroneControlCommand)
            {
                var controlCommand = eventArgs.DroneCommand as DroneControlCommand;
                switch (controlCommand.Type)
                {
                    case DroneControlCommandType.Connect:
                        m_drone.Connect();
                        break;
                }
            }
            else if (eventArgs.DroneCommand is DroneCameraCommand)
            {
                var cameraCommand = eventArgs.DroneCommand as DroneCameraCommand;
                switch (cameraCommand.Type)
                {
                    case DroneCameraCommandType.TurnOn:
                        m_drone.StartStreamingVideo();
                        break;
                    case DroneCameraCommandType.TurnOff:
                        m_drone.StopStreamingVideo();
                        // remove the actual picture from screen
                        break;
                }
            }
            else if (eventArgs.DroneCommand is DroneMovementCommand)
            {
                var movementCommand = eventArgs.DroneCommand as DroneMovementCommand;
                switch (movementCommand.MovementType)
                {
                    case DroneMovementType.TakeOff:
                        m_drone.TakeOff();
                        break;
                    case DroneMovementType.Land:
                        m_drone.Land();
                        break;
                    case DroneMovementType.Move:
                        m_drone.Move(movementCommand.Movement);
                        break;
                }
            }
        }

        private void HandleHandDetectorStateChanged(object source, HandDetectorStateChangedEventArgs eventArgs)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UserInterfaceConnector.HandDetectorStatus = eventArgs.HandDetectorStatus;
            });
        }

        public void HandleKeyEvent(Key key, KeyState status)
        {
            m_droneController.HandleKeyboardInput(key, status);
        }

        public void ConnectToDrone()
        {
            m_droneController.Connect();
        }

        public void TurnOnWebCamera()
        {
            m_webCamera.TurnOn();
        }

        public void TurnOffWebCamera()
        {
            m_webCamera.TurnOff();
        }

        public void StartStreamingDroneVideo()
        {
            m_droneController.StartStreamingVideo();
        }

        public void StopStreamingDroneVideo()
        {
            m_droneController.StopStreamingVideo();
        }

        public void Closed()
        {
            m_webCamera.TurnOff();
            m_drone.StopStreamingVideo();
        }

        private BitmapSource BitmapToBitmapSource(Bitmap bitmap, System.Windows.Media.PixelFormat pixelFormat)
        {
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var bitmapSource = BitmapSource.Create(bitmapData.Width, bitmapData.Height, 96, 96, pixelFormat, null, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }
    }
}