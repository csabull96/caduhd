using Caduhd.Common;
using Caduhd.Controller;
using Caduhd.Controller.InputAnalyzer;
using Caduhd.Controller.InputEvaluator;
using Caduhd.Drone;
using Caduhd.Drone.Command;
using Caduhd.HandsDetector;
using Caduhd.Input.Keyboard;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Input;

namespace Caduhd
{
    public class CaduhdApp : IDisposable
    {
        private const int YES = 1;
        private const int NO = 0;

        private int _isWebCameraFrameProcessorBusy;

        private IHandsAnalyzer _handsAnalyzer;
        private ISkinColorHandsDetector _skinColorHandsDetector;
        private HandsDroneController _handsDroneController;

        private ICaduhdUIConnector _uiConnector;

        public CaduhdApp(
            IHandsAnalyzer handsAnalyzer,
            ISkinColorHandsDetector skinColorHandsDetector,
            AbstractDrone drone,
            IDroneControllerKeyInputEvaluator droneControllerKeyInputEvaluator,
            IDroneControllerHandsInputEvaluator droneControllerHandsInputEvaluator)
        {
            _isWebCameraFrameProcessorBusy = 0;

            _handsAnalyzer = handsAnalyzer;
            _skinColorHandsDetector = skinColorHandsDetector;
            _handsDroneController = new HandsDroneController(drone, droneControllerHandsInputEvaluator, droneControllerKeyInputEvaluator);

        }

        public void AttachUI(ICaduhdUIConnector uiConnector)
        {
            _uiConnector = uiConnector;
        }

        public void Dispose()
        {
            _handsDroneController.Dispose();
        }


        public void Input(KeyInfo keyInfo)
        {
            if (keyInfo.Key == Key.Back)
            {
                if (keyInfo.KeyState == KeyState.Down)
                {
                    if (_skinColorHandsDetector.Tuned)
                    {
                        _skinColorHandsDetector.InvalidateTuning();
                        _handsAnalyzer.Reset();
                    }
                    else
                    {
                        _handsAnalyzer.AdvanceState();
                    }
                }
            }
            else
            {
                _handsDroneController.ProcessKeyInput(keyInfo);
            }
        }

        public void Input(BgrImage image)
        {
            if (Interlocked.CompareExchange(ref _isWebCameraFrameProcessorBusy, YES, NO) == NO)
            {
                BgrImage frame = image;
                NormalizedHands hands = null;
                MoveCommand moveCommand = null;

                if (_skinColorHandsDetector.Tuned)
                {
                    HandsDetectorResult result = _skinColorHandsDetector.DetectHands(frame);
                    frame = result.Image;
                    hands = result.Hands;
                    moveCommand =
                        (_handsDroneController.ProcessHandsInput(hands) as DroneControllerHandsInputProcessResult)?.Result;
                }
                else
                {
                    if (_handsAnalyzer.State == HandsAnalyzerState.ReadyToAnalyzeLeft || _handsAnalyzer.State == HandsAnalyzerState.AnalyzingLeft)
                    {
                        if (_handsAnalyzer.State == HandsAnalyzerState.AnalyzingLeft)
                        {
                            _handsAnalyzer.AnalyzeLeft(frame, _handsDroneController.HandsInputEvaluator.TunerHands["left"]["poi"]);
                            _handsAnalyzer.AdvanceState();
                        }

                        frame.MarkPoints(_handsDroneController.HandsInputEvaluator.TunerHands["left"]["outline"], Color.Yellow);
                    }
                    else if (_handsAnalyzer.State == HandsAnalyzerState.ReadyToAnalyzeRight || _handsAnalyzer.State == HandsAnalyzerState.AnalyzingRight)
                    {
                        if (_handsAnalyzer.State == HandsAnalyzerState.AnalyzingRight)
                        {
                            _handsAnalyzer.AnalyzeRight(frame, _handsDroneController.HandsInputEvaluator.TunerHands["right"]["poi"]);
                            _handsAnalyzer.AdvanceState();
                        }

                        frame.MarkPoints(_handsDroneController.HandsInputEvaluator.TunerHands["right"]["outline"], Color.Yellow);
                    }
                    else if (_handsAnalyzer.State == HandsAnalyzerState.Tuning)
                    {
                        HandsAnalyzerResult result = _handsAnalyzer.Result;
                        NormalizedHands neutralHands = _skinColorHandsDetector.Tune(result);
                        _handsDroneController.HandsInputEvaluator.Tune(neutralHands);
                    }
                }

               _uiConnector?.SetComputerCameraImage(frame);
                _uiConnector?.SetHandsInputEvaluated(moveCommand);

                Interlocked.Exchange(ref _isWebCameraFrameProcessorBusy, NO);
            }
        }

    }
}
