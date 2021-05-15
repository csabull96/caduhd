namespace Caduhd
{
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Input;
    using Caduhd.Common;
    using Caduhd.Controller;
    using Caduhd.Controller.InputAnalyzer;
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Drone;
    using Caduhd.Drone.Command;
    using Caduhd.HandsDetector;
    using Caduhd.Input.Keyboard;

    /// <summary>
    /// The Caduhd App.
    /// </summary>
    public class CaduhdApp : IDisposable
    {
        private const int YES = 1;
        private const int NO = 0;

        private readonly IHandsAnalyzer handsAnalyzer;
        private readonly ISkinColorHandsDetector skinColorHandsDetector;
        private readonly HandsDroneController handsDroneController;

        private int isWebCameraFrameProcessorBusy;

        private ICaduhdUIConnector uiConnector;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaduhdApp"/> class.
        /// </summary>
        /// <param name="handsAnalyzer">A hands analyzer.</param>
        /// <param name="skinColorHandsDetector">A skin color based hands detector.</param>
        /// <param name="drone">A drone.</param>
        /// <param name="droneControllerKeyInputEvaluator">A drone controller key input evaluator.</param>
        /// <param name="droneControllerHandsInputEvaluator">A drone controller hands input evaluator.</param>
        public CaduhdApp(
            IHandsAnalyzer handsAnalyzer,
            ISkinColorHandsDetector skinColorHandsDetector,
            AbstractDrone drone,
            IDroneControllerKeyInputEvaluator droneControllerKeyInputEvaluator,
            IDroneControllerHandsInputEvaluator droneControllerHandsInputEvaluator)
        {
            this.isWebCameraFrameProcessorBusy = 0;

            this.handsAnalyzer = handsAnalyzer;
            this.skinColorHandsDetector = skinColorHandsDetector;

            if (drone is IStreamer streamer)
            {
                streamer.NewCameraFrame += this.StreamerNewCameraFrame;
            }

            if (drone is IStateful stateful)
            {
                stateful.StateChanged += this.StatefulStateChanged;
            }

            this.handsDroneController = new HandsDroneController(drone, droneControllerHandsInputEvaluator, droneControllerKeyInputEvaluator);
        }

        /// <summary>
        /// Binds the Caduhd App to a user interface connector.
        /// </summary>
        /// <param name="uiConnector">The user interface connector.</param>
        public void Bind(ICaduhdUIConnector uiConnector)
        {
            this.uiConnector = uiConnector;
        }

        /// <summary>
        /// Disposing this object.
        /// </summary>
        public void Dispose()
        {
            this.handsDroneController.Dispose();
        }

        /// <summary>
        /// This is the entry point of the key inputs into the Caduhd App.
        /// </summary>
        /// <param name="keyInfo">The key info of the key event.</param>
        public void Input(KeyInfo keyInfo)
        {
            if (keyInfo.Key == Key.Back)
            {
                if (keyInfo.KeyState == KeyState.Down)
                {
                    if (this.skinColorHandsDetector.Tuned)
                    {
                        this.skinColorHandsDetector.InvalidateTuning();
                        this.handsAnalyzer.Reset();
                    }
                    else
                    {
                        this.handsAnalyzer.AdvanceState();
                    }
                }
            }
            else
            {
                this.handsDroneController.ProcessKeyInput(keyInfo);
            }
        }

        /// <summary>
        /// This is the entry point of the hands into the Caduhd App.
        /// </summary>
        /// <param name="image">The image, possibly containing hands.</param>
        public void Input(BgrImage image)
        {
            if (Interlocked.CompareExchange(ref this.isWebCameraFrameProcessorBusy, YES, NO) == NO)
            {
                BgrImage frame = image;
                MoveCommand moveCommand = null;

                if (this.skinColorHandsDetector.Tuned)
                {
                    HandsDetectorResult result = this.skinColorHandsDetector.DetectHands(frame);
                    frame = result.Image;
                    var hands = result.Hands;
                    moveCommand =
                        (this.handsDroneController.ProcessHandsInput(hands) as DroneControllerHandsInputProcessResult)?.Result;
                }
                else
                {
                    if (this.handsAnalyzer.State == HandsAnalyzerState.ReadyToAnalyzeLeft || this.handsAnalyzer.State == HandsAnalyzerState.AnalyzingLeft)
                    {
                        if (this.handsAnalyzer.State == HandsAnalyzerState.AnalyzingLeft)
                        {
                            this.handsAnalyzer.AnalyzeLeft(frame, this.handsDroneController.HandsInputEvaluator.TunerHands["left"]["poi"]);
                            this.handsAnalyzer.AdvanceState();
                        }

                        frame.MarkPoints(this.handsDroneController.HandsInputEvaluator.TunerHands["left"]["outline"], Color.Yellow);
                    }
                    else if (this.handsAnalyzer.State == HandsAnalyzerState.ReadyToAnalyzeRight || this.handsAnalyzer.State == HandsAnalyzerState.AnalyzingRight)
                    {
                        if (this.handsAnalyzer.State == HandsAnalyzerState.AnalyzingRight)
                        {
                            this.handsAnalyzer.AnalyzeRight(frame, this.handsDroneController.HandsInputEvaluator.TunerHands["right"]["poi"]);
                            this.handsAnalyzer.AdvanceState();
                        }

                        frame.MarkPoints(this.handsDroneController.HandsInputEvaluator.TunerHands["right"]["outline"], Color.Yellow);
                    }
                    else if (this.handsAnalyzer.State == HandsAnalyzerState.Tuning)
                    {
                        HandsAnalyzerResult result = this.handsAnalyzer.Result;
                        NormalizedHands neutralHands = this.skinColorHandsDetector.Tune(result);
                        this.handsDroneController.HandsInputEvaluator.Tune(neutralHands);
                    }
                }

                this.uiConnector?.SetComputerCameraImage(frame);
                this.uiConnector?.SetEvaluatedHandsInput(moveCommand);

                Interlocked.Exchange(ref this.isWebCameraFrameProcessorBusy, NO);
            }
        }

        private void StatefulStateChanged(object source, Drone.Event.DroneStateChangedEventArgs args)
        {
            this.uiConnector.SetDroneState(args.DroneState);
        }

        private void StreamerNewCameraFrame(object source, Drone.Event.NewDroneCameraFrameEventArgs args)
        {
            this.uiConnector.SetDroneCameraImage(args.Frame);
        }
    }
}
