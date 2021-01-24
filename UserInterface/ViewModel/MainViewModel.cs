using Caduhd.Controller;
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
        public IWebCamera WebCamera { get; set; }

        public DroneController DroneController { get; private set; }

        private BitmapSource _currentWebCameraFrame;
        public BitmapSource CurrentWebCameraFrame
        {
            get { return _currentWebCameraFrame; }
            set
            {
                _currentWebCameraFrame = value;
                OnPropertyChanged();
            }
        }

        private BitmapSource _currentDroneCameraFrame;
        public BitmapSource CurrentDroneCameraFrame
        {
            get { return _currentDroneCameraFrame; }
            set
            {
                _currentDroneCameraFrame = value;
                OnPropertyChanged();
            }
        }

        private string label;
        public string Label
        {
            get { return label; }
            set 
            { 
                label = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            WebCamera = new WebCamera(30);
            WebCamera.TurnOn();      
            WebCamera.Feed += (s, args) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CurrentWebCameraFrame = BitmapToBitmapSource(args.Frame, PixelFormats.Bgr24);
                });
                DroneController.HandleInput(args.Frame);
            };

            Tello tello = new Tello();

            IHandDetector handDetector = new Caduhd.HandDetector.Detector.HandDetector();
            DroneController = new DroneController(handDetector);
            DroneController.ControlDrone += (s, args) =>
            {
                if (args.DroneCommand is DroneControlCommand)
                {
                    var controlCommand = args.DroneCommand as DroneControlCommand;
                    switch (controlCommand.Type)
                    {
                        case DroneControlCommandType.Connect:
                            tello.Connect();


                            tello.StartVideoStream();
                            tello.Feed += Tello_Feed;
                            break;
                    }
                }
                else if (args.DroneCommand is DroneCameraCommand)
                {
                    var cameraCommand = args.DroneCommand as DroneCameraCommand;
                    switch (cameraCommand.Type)
                    {
                        case DroneCameraCommandType.TurnOn:
                            tello.StartVideoStream();
                            tello.Feed += Tello_Feed;
                            break;
                        case DroneCameraCommandType.TurnOff:
                            tello.StopVideoStream();
                            tello.Feed -= Tello_Feed;
                            break;
                    }
                }
                else if (args.DroneCommand is DroneMovementCommand)
                {
                    var movementCommand = args.DroneCommand as DroneMovementCommand;
                    switch (movementCommand.MovementType)
                    {
                        case DroneMovementCommandType.TakeOff:
                            tello.TakeOff();
                            break;
                        case DroneMovementCommandType.Land:
                            tello.Land();
                            break;
                        case DroneMovementCommandType.Move:
                            tello.Move(movementCommand.Movement);
                            break;
                    }
                }
            };
        }

        private void Tello_Feed(object source, DroneVideoEventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CurrentDroneCameraFrame = BitmapToBitmapSource(args.Frame, PixelFormats.Bgr24);
            });
        }

        public void HandleKeyEvent(Key key, KeyStatus status)
        {
            DroneController.HandleInput(key, status);
        }

        private BitmapSource BitmapToBitmapSource(Bitmap bmp, System.Windows.Media.PixelFormat pixelFormat)
        {
            var bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);

            var bitmapSource = BitmapSource.Create(bitmapData.Width, bitmapData.Height, 96, 96, pixelFormat, null, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bmp.UnlockBits(bitmapData);
            return bitmapSource;
        }
    }
}
