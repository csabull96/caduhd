using Caduhd.Controller;
using Caduhd.Controller.Commands;
using Caduhd.Controller.InputEvaluator;
using Caduhd.Drone;
using Ksvydo.HandDetector;
using Ksvydo.HandDetector.Calibrator;
using Ksvydo.HandDetector.Model;
using Ksvydo.Input.Camera;
using Ksvydo.Input.Keyboard;
using System.Drawing;
using System.Windows;
using System.Windows.Input;

namespace Caduhd.UserInterface.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private ColorBasedHandDetector m_handDetector;
        private Tello m_tello;
        private HandsDroneController m_droneController;
        private KeyEventProcessor m_keyEventProcessor;
        private IWebCamera m_webCamera;

        public UserInterfaceConnector UserInterfaceConnector { get; private set; } = new UserInterfaceConnector();
        
        public MainViewModel()
        {
            m_handDetector = new ColorBasedHandDetector(new ColorCalibrator());
            m_handDetector.StateChanged += HandleHandDetectorStateChanged;
            
            m_tello = new Tello();
            m_tello.StateChanged += HandleDroneStateChanged;
            m_tello.NewCameraFrame += ProcessDroneCameraFrame;

            DroneControllerKeyInputEvaluatorFactory factory = new DroneControllerKeyInputEvaluatorFactory();
            var droneControllerKeyInputEvaluator = factory.GetDroneControllerKeyInputEvaluator(m_tello);
            m_droneController = 
                new HandsDroneController(m_tello, new DroneHandsInputEvaluator(), 
                    droneControllerKeyInputEvaluator);

            m_keyEventProcessor = new KeyEventProcessor();

            m_webCamera = new WebCamera();
            m_webCamera.NewFrame += ProcessWebCameraFrame;
            m_webCamera.TurnOn();
        }

        private void HandleHandDetectorStateChanged()
        {
            UserInterfaceConnector.HandDetectorState = m_handDetector.State;
        }

        public void HandleKeyEvent(KeyEventArgs keyEventArgs)
        {
            KeyInfo keyInfo = m_keyEventProcessor.ProcessKeyEvent(keyEventArgs);

            if (keyInfo.Key == Key.Back)
            {
                if (keyInfo.KeyState == KeyState.Down)
                {
                    m_handDetector.ShiftState();
                }
            }
            else
            {
                m_droneController.ProcessKeyInput(keyInfo);
            }
        }

        private void ProcessWebCameraFrame(object sender, NewWebCameraFrameEventArgs args)
        {
            Bitmap frame = null;
            Hands hands = null;
            MoveCommand moveCommand = null;

            switch (m_handDetector.State)
            {
                case ColorBasedHandDetectorState.NeedsCalibrating:
                case ColorBasedHandDetectorState.NeedsReCalibrating:
                    break;
                case ColorBasedHandDetectorState.ReadyToCaptureBackground:
                    frame = m_handDetector.CaptureBackground(args.Frame);               
                    break;
                case ColorBasedHandDetectorState.ReadyToAnalyzeLeftHand:
                    frame = m_handDetector.AnalyzeLeftHand(args.Frame);
                    break;
                case ColorBasedHandDetectorState.ReadyToAnalyzeRightHand:
                    frame = m_handDetector.AnalyzeRightHand(args.Frame);
                    break;
                case ColorBasedHandDetectorState.Calibrated:
                    frame = m_handDetector.Update(args.Frame);
                    break;
                case ColorBasedHandDetectorState.Enabled:
                    HandDetectionResult handDetectionResult = m_handDetector.DetectHands(args.Frame);
                    frame = handDetectionResult.Frame;
                    hands = handDetectionResult.Hands;

                    InputProcessResult result = m_droneController.ProcessHandsInput(hands);
                    moveCommand = (result as DroneControllerHandsInputProcessResult).Result;
                    break;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                UserInterfaceConnector.SetCurrentWebCameraFrame(frame ?? args.Frame);

                // in this way
                // the following values are forwarded to the ui only for debugging purposes (as a developer feedback)

                UserInterfaceConnector.LeftHand = hands == null ? "0/0/0" :
                    $"{hands.Left.Position.X}/{hands.Left.Position.Y}/{hands.Left.Weight}";

                UserInterfaceConnector.RightHand = hands == null ? "0/0/0" :
                    $"{hands.Right.Position.X}/{hands.Right.Position.Y}/{hands.Right.Weight}";

                UserInterfaceConnector.Direction = moveCommand == null ? "l/r:0 f/b:0 u/d:0 yaw:0" :
                    $"l/r:{moveCommand.Lateral} f/b:{moveCommand.Longitudinal} u/d:{moveCommand.Vertical} yaw:{moveCommand.Yaw}";
            });
        }

        private void HandleDroneStateChanged(object source, DroneStateChangedEventArgs args)
        {
            UserInterfaceConnector.SetSpeed(args.DroneState.Speed);
            UserInterfaceConnector.SetHeight(args.DroneState.Height);
            UserInterfaceConnector.SetBatteryLevel(args.DroneState.Battery);
        }

        private void ProcessDroneCameraFrame(object source, NewDroneCameraFrameEventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UserInterfaceConnector.SetCurrentDroneCameraFrame(args.Frame);
            });
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
            m_droneController.StopStreamingVideo();
        }
    }
}