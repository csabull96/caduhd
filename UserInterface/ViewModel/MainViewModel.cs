namespace Caduhd.UserInterface.ViewModel
{
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using Caduhd.Common;
    using Caduhd.Controller;
    using Caduhd.Controller.Command;
    using Caduhd.Controller.InputAnalyzer;
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Drone;
    using Caduhd.Drone.Dji;
    using Caduhd.Drone.Event;
    using Caduhd.HandsDetector;
    using Caduhd.Input.Camera;
    using Caduhd.Input.Keyboard;

    /// <summary>
    /// Main view model.
    /// </summary>
    public class MainViewModel : BaseViewModel, IDisposable
    {
        private const int YES = 1;
        private const int NO = 0;

        private readonly HandsAnalyzer handsInputAnalyzer;
        private readonly DroneHandsInputEvaluator droneHandsInputEvaluator;
        private readonly SkinColorHandsDetector handsDetector;
        private readonly Tello tello;
        private readonly HandsDroneController droneController;

        private readonly IWebCamera webCamera;
        private readonly KeyEventProcessor keyEventProcessor;

        private int isWebCameraFrameProcessorBusy;

        /// <summary>
        /// Gets the user interface connector for data binding between this view model and the UI.
        /// </summary>
        public UserInterfaceConnector UserInterfaceConnector { get; private set; } = new UserInterfaceConnector();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            this.isWebCameraFrameProcessorBusy = 0;

            this.handsInputAnalyzer = new HandsAnalyzer();
            this.droneHandsInputEvaluator = new DroneHandsInputEvaluator();
            this.handsDetector = new SkinColorHandsDetector();
            this.tello = new Tello();
            this.tello.StateChanged += this.HandleDroneStateChanged;
            this.tello.NewCameraFrame += this.ProcessDroneCameraFrame;
            DroneControllerKeyInputEvaluatorFactory factory = new DroneControllerKeyInputEvaluatorFactory();
            IDroneKeyInputEvaluator droneControllerKeyInputEvaluator = factory.GetDroneControllerKeyInputEvaluator(this.tello);
            this.droneController =
                new HandsDroneController(this.tello, this.droneHandsInputEvaluator, droneControllerKeyInputEvaluator);

            this.webCamera = new WebCamera(320, 280);
            this.webCamera.NewFrame += this.ProcessWebCameraFrame;
            this.webCamera.On();
            this.keyEventProcessor = new KeyEventProcessor();
        }

        /// <summary>
        /// Handles the key events coming from the UI.
        /// </summary>
        /// <param name="keyEventArgs">The key event arguments.</param>
        public void HandleKeyEvent(KeyEventArgs keyEventArgs)
        {
            KeyInfo keyInfo = this.keyEventProcessor.ProcessKeyEvent(keyEventArgs.Key, keyEventArgs.IsDown, keyEventArgs.IsRepeat);

            if (keyInfo.Key == Key.Back)
            {
                if (keyInfo.KeyState == KeyState.Down)
                {
                    if (this.handsDetector.Tuned)
                    {
                        this.handsDetector.InvalidateTuning();
                        this.handsInputAnalyzer.Reset();
                    }
                    else
                    {
                        this.handsInputAnalyzer.AdvanceState();
                    }
                }
            }
            else
            {
                this.droneController.ProcessKeyInput(keyInfo);
            }
        }

        /// <summary>
        /// Tells the drone controller to connect to the drone.
        /// </summary>
        public void ConnectToDrone()
        {
            this.droneController.Connect();
        }

        /// <summary>
        /// Tells the drone controller to turn on the drone's camera.
        /// </summary>
        public void StartStreamingDroneVideo()
        {
            this.droneController.StartStreamingVideo();
        }

        /// <summary>
        /// Tells the drone controller to turn off the drone's camera.
        /// </summary>
        public void StopStreamingDroneVideo()
        {
            this.droneController.StopStreamingVideo();
        }

        /// <summary>
        /// Disposes the <see cref="MainViewModel"/>.
        /// </summary>
        public void Dispose()
        {
            (this.webCamera as IDisposable).Dispose();
            this.tello.Dispose();
        }

        private void ProcessWebCameraFrame(object sender, NewWebCameraFrameEventArgs args)
        {
            // the web camera frame processor's behaviour strongly depends on its state
            // only 1 thread is allowed to execute the method at a time
            // other threads could change the state (without the original thread's approval)
            if (Interlocked.CompareExchange(ref this.isWebCameraFrameProcessorBusy, YES, NO) == NO)
            {
                BgrImage frame = args.Frame;
                NormalizedHands hands = null;
                MoveCommand moveCommand = null;

                if (this.handsDetector.Tuned)
                {
                    HandsDetectorResult result = this.handsDetector.DetectHands(frame);
                    frame = result.Image;
                    hands = result.Hands;
                    moveCommand =
                        (this.droneController.ProcessHandsInput(hands) as DroneControllerHandsInputProcessResult)?.Result;
                }
                else
                {
                    if (this.handsInputAnalyzer.State == HandsAnalyzerState.ReadyToAnalyzeLeft ||
                        this.handsInputAnalyzer.State == HandsAnalyzerState.AnalyzingLeft)
                    {
                        Rectangle roi = this.droneHandsInputEvaluator.LeftNeutralHandArea(frame.Width, frame.Height);

                        if (this.handsInputAnalyzer.State == HandsAnalyzerState.AnalyzingLeft)
                        {
                            this.handsInputAnalyzer.AnalyzeLeft(frame, roi);
                            this.handsInputAnalyzer.AdvanceState();
                        }

                        frame.DrawRectangle(roi, Color.Red, 2);
                    }
                    else if (this.handsInputAnalyzer.State == HandsAnalyzerState.ReadyToAnalyzeRight ||
                        this.handsInputAnalyzer.State == HandsAnalyzerState.AnalyzingRight)
                    {
                        Rectangle roi = this.droneHandsInputEvaluator.RightNeutralHandArea(frame.Width, frame.Height);

                        if (this.handsInputAnalyzer.State == HandsAnalyzerState.AnalyzingRight)
                        {
                            this.handsInputAnalyzer.AnalyzeRight(frame, roi);
                            this.handsInputAnalyzer.AdvanceState();
                        }

                        frame.DrawRectangle(roi, Color.Red, 2);
                    }
                    else if (this.handsInputAnalyzer.State == HandsAnalyzerState.Tuning)
                    {
                        HandsAnalyzerResult result = this.handsInputAnalyzer.Result;
                        NormalizedHands neutralHands = this.handsDetector.Tune(result);
                        this.droneHandsInputEvaluator.Tune(neutralHands);
                    }
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.UserInterfaceConnector.SetCurrentWebCameraFrame(frame?.Bitmap ?? args.Frame.Bitmap);

                    // developer feedback, debugging
                    this.UserInterfaceConnector.LeftHand = hands == null ? "0/0/0" :
                        $"{hands.Left.X:F2}/{hands.Left.Y:F2}/{hands.Left.Weight:F2}";

                    this.UserInterfaceConnector.RightHand = hands == null ? "0/0/0" :
                        $"{hands.Right.X:F2}/{hands.Right.Y:F2}/{hands.Right.Weight:F2}";

                    this.UserInterfaceConnector.Direction = moveCommand == null ? "l/r:0 f/b:0 u/d:0 yaw:0" :
                        $"l/r:{moveCommand.Lateral} f/b:{moveCommand.Longitudinal} u/d:{moveCommand.Vertical} yaw:{moveCommand.Yaw}";
                });

                Interlocked.Exchange(ref this.isWebCameraFrameProcessorBusy, NO);
            }
        }

        private void HandleDroneStateChanged(object source, DroneStateChangedEventArgs args)
        {
            this.UserInterfaceConnector.SetSpeed(args.DroneState.Wifi);
            this.UserInterfaceConnector.SetHeight(args.DroneState.ToF);
            this.UserInterfaceConnector.SetBatteryLevel(args.DroneState.Battery);
        }

        private void ProcessDroneCameraFrame(object source, NewDroneCameraFrameEventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.UserInterfaceConnector.SetCurrentDroneCameraFrame(args.Frame.Bitmap);
            });
        }
    }
}