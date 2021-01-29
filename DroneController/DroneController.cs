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
        private InputKeys m_inputKeys;
        private IHandDetector m_handDetector;
        private AbstractDroneCommand m_latestDroneCommandFromKeyboard;
        private AbstractDroneCommand m_latestDroneCommandFromWebCamera;

        public delegate void InputEvaluatedEventHandler(object source, DroneControllerInputEvaluatedEventArgs eventArgs);
        public event InputEvaluatedEventHandler InputEvaluated;

        public DroneController(IHandDetector handDetector)
        {
            m_inputKeys = new InputKeys();
            m_handDetector = handDetector;
        }

        public void HandleWebCameraInput(Bitmap frame)
        {
            // should be an async method?
            Hands handsDetected = m_handDetector.DetectHands(frame);
            m_latestDroneCommandFromWebCamera = EvaluateDetectedHands(handsDetected);
            EvaluateCommands();
        }

        public void HandleKeyboardInput(Key key, KeyState keyState)
        {
            if (m_inputKeys.TryUpdate(key, keyState))
            {
                m_latestDroneCommandFromKeyboard = EvaluateInputKeys(m_inputKeys.Keys);
                EvaluateCommands();       
            }         
        }

        private AbstractDroneCommand EvaluateDetectedHands(Hands hands)
        {
            // todo
            return null;
        }

        private AbstractDroneCommand EvaluateInputKeys(IDictionary<Key, KeyState> inputKeyStates)
        {
            DroneMovement movement = new DroneMovement();
            int speed = 60;

            if (inputKeyStates[Key.Back] == KeyState.Down)
            {
                return new DroneMovementCommand(DroneMovementCommandType.Land, movement);
            }
            else if (inputKeyStates[Key.Enter] == KeyState.Down)
            {
                return new DroneMovementCommand(DroneMovementCommandType.TakeOff, movement);
            }
            else
            {
                if (inputKeyStates[Key.W] == KeyState.Down)
                {
                    movement.Vertical += speed;
                }
                if (inputKeyStates[Key.S] == KeyState.Down)
                {
                    movement.Vertical -= speed;
                }
                if (inputKeyStates[Key.A] == KeyState.Down)
                {
                    movement.Yaw -= speed;
                }
                if (inputKeyStates[Key.D] == KeyState.Down)
                {
                    movement.Yaw += speed;
                }
                if (inputKeyStates[Key.Up] == KeyState.Down)
                {
                    movement.Longitudinal += speed;
                }
                if (inputKeyStates[Key.Down] == KeyState.Down)
                {
                    movement.Longitudinal -= speed;
                }
                if (inputKeyStates[Key.Left] == KeyState.Down)
                {
                    movement.Lateral -= speed;
                }
                if (inputKeyStates[Key.Right] == KeyState.Down)
                {
                    movement.Lateral += speed;
                }

                return new DroneMovementCommand(DroneMovementCommandType.Move, movement);
            }
        }

        private void EvaluateCommands()
        {
            // evaluate the m_latestDroneCommandFromWebCamera and the m_latestDroneCommandFromKeyboard together
            InputEvaluated?.Invoke(this, new DroneControllerInputEvaluatedEventArgs(m_latestDroneCommandFromKeyboard));
        }

        public void Connect()
        {
            var connectCommand = new DroneControlCommand(DroneControlCommandType.Connect);
            InputEvaluated?.Invoke(this, new DroneControllerInputEvaluatedEventArgs(connectCommand));
        }

        public void TakeOff()
        {
            var takeOffCommand = new DroneMovementCommand(DroneMovementCommandType.TakeOff);
            InputEvaluated?.Invoke(this, new DroneControllerInputEvaluatedEventArgs(takeOffCommand));
        }

        public void Land()
        {
            var landCommand = new DroneMovementCommand(DroneMovementCommandType.Land);
            InputEvaluated?.Invoke(this, new DroneControllerInputEvaluatedEventArgs(landCommand));
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
