using Caduhd.Controller.Commands;
using Caduhd.Drone;
using Caduhd.HandDetector.Detector;
using Caduhd.HandDetector.Model;
using Caduhd.Input.Camera;
using Caduhd.Input.Keyboard;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Caduhd.Controller
{
    public class DroneController : IWebCameraInputHandler, IKeyboardInputHandler
    {
        // min:0 max:100
        private const int SPEED = 60;

        private InputKeys m_inputKeys;
        private IHandDetector m_handDetector;

        private AbstractDroneCommand m_latestDroneCommandFromKeyboard;
        private DroneMovementCommand m_latestDroneMovementCommandFromWebCamera;

        public delegate void DroneControllerInputEvaluatedEventHandler(object source, DroneControllerInputEvaluatedEventArgs eventArgs);
        public event DroneControllerInputEvaluatedEventHandler InputEvaluated;

        public event HandDetectorStateChangedEventHandler HandDetectorStateChanged;
        public event HandDetectorInputProcessedEventHandler WebCameraFrameProcessed;

        // this is only here for now, for debugging purposes
        public delegate void HandsDetectedEventHandler(object sender, HandsDetectedEventArgs eventArgs);
        public event HandsDetectedEventHandler HandsDetected;

        public DroneController(IHandDetector handDetector)
        {
            m_inputKeys = new InputKeys();
            m_handDetector = handDetector;
            m_handDetector.InputProcessed += (s, args) =>
            {
                WebCameraFrameProcessed?.Invoke(s, args);
            };
            m_handDetector.StateChanged += (s, args) =>
            {
                // be careful here because i intended to hide the hand controller from outside of the drone controler
                // but if we pass forward as event source we can access it from outside
                // but even with the object that we creatae with from the view model so 
                // maybe i should rethink this hiding thing

                HandDetectorStateChanged?.Invoke(s, args);
            };
        }

        public void HandleKeyboardInput(Key key, KeyState keyState)
        {
            if (m_inputKeys.TryUpdate(key, keyState))
            {
                m_latestDroneCommandFromKeyboard = EvaluateInputKeys(m_inputKeys.Keys);
                Evaluate();       
            }         
        }

        public void HandleWebCameraInput(Bitmap frame)
        {
            switch (m_handDetector.State)
            {
                case HandDetectorState.NeedsCalibrating:
                case HandDetectorState.NeedsReCalibrating:
                case HandDetectorState.ReadyToCaptureBackground:
                    m_handDetector.CaptureBackground(frame);
                    WebCameraFrameProcessed?.Invoke(this, new InputProcessedEventArgs(frame));
                    break;
                case HandDetectorState.ReadyToAnalyzeLeftHand:
                    m_handDetector.AnalyzeLeftHand(frame);
                    break;
                case HandDetectorState.ReadyToAnalyzeRightHand:
                    m_handDetector.AnalyzeRightHand(frame);
                    break;
                case HandDetectorState.Calibrated:
                    WebCameraFrameProcessed?.Invoke(this, new InputProcessedEventArgs(frame));
                    break;
                case HandDetectorState.Enabled:
                    Hands handsDetected = m_handDetector.DetectHands(frame);
                    m_latestDroneMovementCommandFromWebCamera = EvaluateHands(handsDetected);
                    Evaluate();
                    break;
            }  
        }

        private AbstractDroneCommand EvaluateInputKeys(IDictionary<Key, KeyState> inputKeyStates)
        {
            if (inputKeyStates[Key.Back] == KeyState.Down)
            {
                m_handDetector.ShiftState();
            }

            if (inputKeyStates[Key.Enter] == KeyState.Down)
            {
                return new DroneMovementCommand(DroneMovementType.TakeOff, DroneMovement.Idle);
            }
            else if (inputKeyStates[Key.Space] == KeyState.Down)
            {
                return new DroneMovementCommand(DroneMovementType.Land, DroneMovement.Idle);
            }
            else
            {
                DroneMovement movement = new DroneMovement();

                if (inputKeyStates[Key.W] == KeyState.Down)
                {
                    movement.Vertical += SPEED;
                }
                if (inputKeyStates[Key.S] == KeyState.Down)
                {
                    movement.Vertical -= SPEED;
                }
                if (inputKeyStates[Key.A] == KeyState.Down)
                {
                    movement.Yaw -= SPEED;
                }
                if (inputKeyStates[Key.D] == KeyState.Down)
                {
                    movement.Yaw += SPEED;
                }
                if (inputKeyStates[Key.Up] == KeyState.Down)
                {
                    movement.Longitudinal += SPEED;
                }
                if (inputKeyStates[Key.Down] == KeyState.Down)
                {
                    movement.Longitudinal -= SPEED;
                }
                if (inputKeyStates[Key.Left] == KeyState.Down)
                {
                    movement.Lateral -= SPEED;
                }
                if (inputKeyStates[Key.Right] == KeyState.Down)
                {
                    movement.Lateral += SPEED;
                }

                return new DroneMovementCommand(DroneMovementType.Move, movement);
            }
        }

        private DroneMovementCommand EvaluateHands(Hands hands)
        {
            DroneMovement movement = new DroneMovement();

            if (1000 < hands.Left.Weight && 1000 < hands.Right.Weight)
            {
                //longitudinal
                if (4500 < hands.Left.Weight && 4500 < hands.Right.Weight)
                {
                    movement.Longitudinal += SPEED;
                }
                else if (hands.Left.Weight < 3000 && hands.Right.Weight < 3000)
                {
                    movement.Longitudinal -= SPEED;
                }

                //lateral
                if (Math.Abs(hands.Left.Position.Y - hands.Right.Position.Y) > 50)
                {
                    if (hands.Left.Position.Y > hands.Right.Position.Y)
                    {
                        movement.Lateral -= SPEED;
                    }
                    else
                    {
                        movement.Lateral += SPEED;
                    }
                }

                // yaw
                if (Math.Abs(hands.Left.Weight - hands.Right.Weight) > 2500)
                {
                    if (hands.Left.Weight > hands.Right.Weight)
                    {
                        movement.Yaw += SPEED;
                    }
                    else
                    {
                        movement.Yaw -= SPEED;
                    }
                }      
            }

            // vertical
            if (hands.Left.Weight > 1000 && hands.Right.Weight > 1000)
            {
                if (hands.Left.Position.Y < 50 && hands.Right.Position.Y < 50)
                {
                    movement.Vertical += SPEED;
                }
                else if (140 < hands.Left.Position.Y && 140 < hands.Right.Position.Y)
                {
                    movement.Vertical -= SPEED;
                }
            }
            
            string toString = $"l/r:{movement.Lateral} f/b:{movement.Longitudinal} u/d:{movement.Vertical} yaw:{movement.Yaw}";
            HandsDetected?.Invoke(hands, new HandsDetectedEventArgs(hands, toString));

            return new DroneMovementCommand(DroneMovementType.Move, movement);
        }

        private void Evaluate()
        {
            if (m_latestDroneCommandFromKeyboard is DroneMovementCommand)
            {
                DroneMovementCommand droneMovementCommand = m_latestDroneCommandFromKeyboard as DroneMovementCommand;

                if (droneMovementCommand.MovementType == DroneMovementType.TakeOff 
                    || droneMovementCommand.MovementType == DroneMovementType.Land
                    || (droneMovementCommand.MovementType == DroneMovementType.Move && droneMovementCommand.Movement.Moving))
                {
                    InputEvaluated?.Invoke(this, new DroneControllerInputEvaluatedEventArgs(m_latestDroneCommandFromKeyboard));
                }
                else if (droneMovementCommand.MovementType == DroneMovementType.Move)
                {
                    if (droneMovementCommand.Movement.Moving
                        || (droneMovementCommand.Movement.Still && m_latestDroneMovementCommandFromWebCamera == null))
                    {
                        InputEvaluated?.Invoke(this, new DroneControllerInputEvaluatedEventArgs(m_latestDroneCommandFromKeyboard));
                    }
                    else
                    {
                        var copied = m_latestDroneMovementCommandFromWebCamera.Copy();
                        m_latestDroneMovementCommandFromWebCamera = null;
                        InputEvaluated?.Invoke(this, new DroneControllerInputEvaluatedEventArgs(copied));
                    }
                }


               
            }
            else if (m_latestDroneCommandFromKeyboard is DroneControlCommand || m_latestDroneCommandFromKeyboard is DroneCameraCommand)
            {
                InputEvaluated?.Invoke(this, new DroneControllerInputEvaluatedEventArgs(m_latestDroneCommandFromKeyboard));
            }
        }

        public void Connect()
        {
            var connectCommand = new DroneControlCommand(DroneControlCommandType.Connect);
            InputEvaluated?.Invoke(this, new DroneControllerInputEvaluatedEventArgs(connectCommand));
        }

        public void StartStreamingVideo()
        {
            var startStreamingVideoCommand = new DroneCameraCommand(DroneCameraCommandType.TurnOn);
            InputEvaluated?.Invoke(this, new DroneControllerInputEvaluatedEventArgs(startStreamingVideoCommand));
        }

        public void StopStreamingVideo()
        {
            var stopStreamingVideoCommand = new DroneCameraCommand(DroneCameraCommandType.TurnOff);
            InputEvaluated?.Invoke(this, new DroneControllerInputEvaluatedEventArgs(stopStreamingVideoCommand));
        }
    }
}
