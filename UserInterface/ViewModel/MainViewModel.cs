using Caduhd.Controller;
using Caduhd.Controller.Commands;
using Caduhd.Drone;
using Caduhd.HandDetector.Detector;
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
        private IWebCamera m_webCamera = new WebCamera();
        private AbstractDrone m_drone = new Tello();
        private IHandDetector m_handDetector = new Caduhd.HandDetector.Detector.HandDetector();
        private DroneController m_droneController;

        public MainViewModel()
        {
            m_webCamera.NewFrame += HandleWebCameraFrame;
            m_drone.NewDroneCameraFrame += HandleDroneCameraFrame;
            m_drone.StateChanged += HandleDroneStateChanged;
            m_droneController = new DroneController(m_handDetector);
            m_droneController.InputEvaluated += ControlDrone;
        }

        private void HandleWebCameraFrame(object sender, NewWebCameraFrameEventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UserInterfaceConnector.CurrentWebCameraFrame = BitmapToBitmapSource(args.Frame, PixelFormats.Bgr24);
            });

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
                    case DroneMovementCommandType.TakeOff:
                        m_drone.TakeOff();
                        break;
                    case DroneMovementCommandType.Land:
                        m_drone.Land();
                        break;
                    case DroneMovementCommandType.Move:
                        m_drone.Move(movementCommand.Movement);
                        break;
                }
            }
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

        private BitmapSource BitmapToBitmapSource(Bitmap bitmap, System.Windows.Media.PixelFormat pixelFormat)
        {
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var bitmapSource = BitmapSource.Create(bitmapData.Width, bitmapData.Height, 96, 96, pixelFormat, null, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }
    }
}