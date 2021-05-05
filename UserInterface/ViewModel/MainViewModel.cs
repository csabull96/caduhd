using Caduhd.Common;
using Caduhd.Controller;
using Caduhd.Controller.Command;
using Caduhd.Controller.InputAnalyzer;
using Caduhd.Controller.InputEvaluator;
using Caduhd.Drone;
using Caduhd.Drone.Event;
using Caduhd.HandsDetector;
using Caduhd.Input.Camera;
using Caduhd.Input.Keyboard;
using System.Drawing;
using System.Windows;
using System.Windows.Input;

namespace Caduhd.UserInterface.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private bool _isWebCameraFrameProcessorBusy;

        private readonly HandsAnalyzer _handsInputAnalyzer;
        private readonly DroneHandsInputEvaluator _droneHandsInputEvaluator;
        private readonly SkinColorHandsDetector _handsDetector;
        private readonly Tello _tello;
        private readonly HandsDroneController _droneController;

        private readonly IWebCamera _webCamera;
        private readonly KeyEventProcessor _keyEventProcessor;

        public UserInterfaceConnector UserInterfaceConnector { get; private set; } = new UserInterfaceConnector();
        
        public MainViewModel()
        {
            _isWebCameraFrameProcessorBusy = false;

            _handsInputAnalyzer = new HandsAnalyzer();
            _droneHandsInputEvaluator = new DroneHandsInputEvaluator();
            _handsDetector = new SkinColorHandsDetector();
            _tello = new Tello();
            _tello.StateChanged += HandleDroneStateChanged;
            _tello.NewCameraFrame += ProcessDroneCameraFrame;
            DroneControllerKeyInputEvaluatorFactory factory = new DroneControllerKeyInputEvaluatorFactory();
            IDroneKeyInputEvaluator droneControllerKeyInputEvaluator = 
                factory.GetDroneControllerKeyInputEvaluator(_tello);
            _droneController = 
                new HandsDroneController(_tello, _droneHandsInputEvaluator, droneControllerKeyInputEvaluator);

            _webCamera = new WebCamera(320, 180);
            _webCamera.NewFrame += ProcessWebCameraFrame;
            _webCamera.TurnOn();
            _keyEventProcessor = new KeyEventProcessor();
        }

        public void HandleKeyEvent(KeyEventArgs keyEventArgs)
        {
            KeyInfo keyInfo = _keyEventProcessor.ProcessKeyEvent(keyEventArgs);

            if (keyInfo.Key == Key.Back)
            {
                if (keyInfo.KeyState == KeyState.Down)
                {
                    if (_handsDetector.Tuned)
                    {
                        _handsDetector.InvalidateTuning();
                        _handsInputAnalyzer.Reset();
                    }
                    else
                    {
                        _handsInputAnalyzer.AdvanceState();
                    }
                }
            }
            else
            {
                _droneController.ProcessKeyInput(keyInfo);
            }
        }

        private void ProcessWebCameraFrame(object sender, NewWebCameraFrameEventArgs args)
        {
            // the web camera frame processor's behaviour strongly depends on its state
            // only 1 thread is allowed to execute the method at a time
            // other threads could change the state, too (without the original thread acknowledging it)
            if (!_isWebCameraFrameProcessorBusy)
            {
                _isWebCameraFrameProcessorBusy = true;
                
                BgrImage frame = args.Frame;
                NormalizedHands hands = null;
                MoveCommand moveCommand = null;

                if (_handsDetector.Tuned)
                {
                    HandsDetectorResult result = _handsDetector.DetectHands(frame);
                    frame = result.Image;
                    hands = result.Hands;
                    moveCommand = 
                        (_droneController.ProcessHandsInput(hands) as DroneControllerHandsInputProcessResult)?.Result;
                }
                else
                {
                    if (_handsInputAnalyzer.State == HandsAnalyzerState.ReadyToAnalyzeLeft || _handsInputAnalyzer.State == HandsAnalyzerState.AnalyzingLeft)
                    {
                        Rectangle roi = _droneHandsInputEvaluator.GetLeftNeutralHandArea(frame.Width, frame.Height);

                        if (_handsInputAnalyzer.State == HandsAnalyzerState.AnalyzingLeft)
                        {
                            _handsInputAnalyzer.AnalyzeLeft(frame, roi);
                        }

                        frame.DrawRectangle(roi, Color.Red, 2);
                    }
                    else if (_handsInputAnalyzer.State == HandsAnalyzerState.ReadyToAnalyzeRight || _handsInputAnalyzer.State == HandsAnalyzerState.AnalyzingRight)
                    {
                        Rectangle roi = _droneHandsInputEvaluator.GetRightNeutralHandArea(frame.Width, frame.Height);

                        if (_handsInputAnalyzer.State == HandsAnalyzerState.AnalyzingRight)
                        {
                            _handsInputAnalyzer.AnalyzeRight(frame, roi);
                        }

                        frame.DrawRectangle(roi, Color.Red, 2);
                    }
                    else if (_handsInputAnalyzer.State == HandsAnalyzerState.Tuning)
                    {
                        HandsInputAnalyzerResult result = _handsInputAnalyzer.Result;
                        NormalizedHands neutralHands = _handsDetector.Tune(result);
                        _droneHandsInputEvaluator.Tune(neutralHands);
                    }
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    UserInterfaceConnector.SetCurrentWebCameraFrame(frame?.Bitmap ?? args.Frame.Bitmap);

                    // developer feedback, debugging
                    UserInterfaceConnector.LeftHand = hands == null ? "0/0/0" :
                        $"{hands.Left.X:F2}/{hands.Left.Y:F2}/{hands.Left.Weight:F2}";

                    UserInterfaceConnector.RightHand = hands == null ? "0/0/0" :
                        $"{hands.Right.X:F2}/{hands.Right.Y:F2}/{hands.Right.Weight:F2}";

                    UserInterfaceConnector.Direction = moveCommand == null ? "l/r:0 f/b:0 u/d:0 yaw:0" :
                        $"l/r:{moveCommand.Lateral} f/b:{moveCommand.Longitudinal} u/d:{moveCommand.Vertical} yaw:{moveCommand.Yaw}";
                });

                _isWebCameraFrameProcessorBusy = false;
            }
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
                UserInterfaceConnector.SetCurrentDroneCameraFrame(args.Frame.Bitmap);
            });
        }

        public void ConnectToDrone()
        {
            _droneController.Connect();
        }

        public void TurnOnWebCamera()
        {
            _webCamera.TurnOn();
        }

        public void TurnOffWebCamera()
        {
            _webCamera.TurnOff();
        }

        public void StartStreamingDroneVideo()
        {
            _droneController.StartStreamingVideo();
        }

        public void StopStreamingDroneVideo()
        {
            _droneController.StopStreamingVideo();
        }

        public void Closed()
        {
            _webCamera.TurnOff();
            _droneController.StopStreamingVideo();
        }
    }
}