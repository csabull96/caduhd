using Caduhd.Drone;
using Caduhd.HandDetector.Detector;
using Caduhd.Input.Camera;
using Caduhd.Input.Keyboard;
using Input.Keyboard;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Caduhd.Controller
{
    public class DroneController : ICameraControlled, IKeyboardControlled
    {
        private IHandDetector m_handDetector;

        public delegate void ControlDroneEventHandler(object source, DroneControllerEventArgs eventArgs);
        public event ControlDroneEventHandler ControlDrone;


        private InputKeys m_inputKeys;


        private AbstractDroneCommand m_latestDroneCommandFromKeyboard;
        private AbstractDroneCommand m_latestDroneCommandFromCamera;

        public DroneController(IHandDetector handDetector)
        {
            m_inputKeys = new InputKeys();
            m_inputKeys.KeyStatusChanged += (s, args) =>
            {
                
            };
            m_handDetector = handDetector;
        }

        public void HandleInput(Bitmap frame)
        {
            // 1) pre process frame
            // 2) detect hands
            // 3) convert hands to command - save this to a member field (droneCommandByHands)

            // evaluate the droneCommandByHands and droneCommandByKeyboard together and invoke a proper ControlDrone event
            EvaluateCommands();
        }

        public void HandleInput(Key key, KeyStatus keyStatus)
        {
            if (m_inputKeys.IsInputKey(key))
            {
                m_inputKeys.UpdateKeyState(key, keyStatus);
                m_latestDroneCommandFromKeyboard = ConvertInputKeys(m_inputKeys.Keys);
                EvaluateCommands();       
            }         
        }

        public AbstractDroneCommand ConvertInputKeys(IDictionary<Key, KeyStatus> inputKeyStates)
        {
            IDroneMovement movement = new DroneMovement();
            int speed = 60;

            if (inputKeyStates[Key.Back] == KeyStatus.Down)
            {
                return new DroneMovementCommand(DroneMovementCommandType.Land, movement);
            }
            else if (inputKeyStates[Key.Enter] == KeyStatus.Down)
            {
                return new DroneMovementCommand(DroneMovementCommandType.TakeOff, movement);
            }
            else
            {
                if (inputKeyStates[Key.W] == KeyStatus.Down)
                {
                    movement.Vertical += speed;
                }
                if (inputKeyStates[Key.S] == KeyStatus.Down)
                {
                    movement.Vertical -= speed;
                }
                if (inputKeyStates[Key.A] == KeyStatus.Down)
                {
                    movement.Yaw -= speed;
                }
                if (inputKeyStates[Key.D] == KeyStatus.Down)
                {
                    movement.Yaw += speed;
                }
                if (inputKeyStates[Key.Up] == KeyStatus.Down)
                {
                    movement.Longitudinal += speed;
                }
                if (inputKeyStates[Key.Down] == KeyStatus.Down)
                {
                    movement.Longitudinal -= speed;
                }
                if (inputKeyStates[Key.Left] == KeyStatus.Down)
                {
                    movement.Lateral -= speed;
                }
                if (inputKeyStates[Key.Right] == KeyStatus.Down)
                {
                    movement.Lateral += speed;
                }

                return new DroneMovementCommand(DroneMovementCommandType.Move, movement);
            }
        }

        private void EvaluateCommands()
        {

            ControlDrone?.Invoke(this, new DroneControllerEventArgs(m_latestDroneCommandFromKeyboard));
        }

        public void Connect()
        {
            var connectCommand = new DroneControlCommand(DroneControlCommandType.Connect);
            ControlDrone?.Invoke(this, new DroneControllerEventArgs(connectCommand));
        }

        public void TakeOff()
        {
            var takeOffCommand = new DroneMovementCommand(DroneMovementCommandType.TakeOff);
            ControlDrone?.Invoke(this, new DroneControllerEventArgs(takeOffCommand));
        }

        public void Land()
        {
            var landCommand = new DroneMovementCommand(DroneMovementCommandType.Land);
            ControlDrone?.Invoke(this, new DroneControllerEventArgs(landCommand));
        }

    }
}
