using Caduhd.Controller.Commands;
using Ksvydo.Input.Keyboard;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace Caduhd.Controller.InputEvaluator
{
    public class GeneralDroneKeyInputEvaluator : AbstractDroneInputEvaluator, IDroneKeyInputEvaluator
    {
        private const Key MOVE_FORWARD_KEY = Key.Up;
        private const Key MOVE_BACKWARD_KEY = Key.Down;
        private const Key MOVE_RIGHT_KEY = Key.Right;
        private const Key MOVE_LEFT_KEY = Key.Left;
        private const Key MOVE_UP_KEY = Key.W;
        private const Key MOVE_DOWN_KEY = Key.S;
        private const Key ROTATE_RIGHT_KEY = Key.D;
        private const Key ROTATE_LEFT_KEY = Key.A;
        private const Key TAKE_OFF_KEY = Key.Enter;
        private const Key LAND_KEY = Key.Space;

        protected KeyInfo MoveForward { get; set; } = new KeyInfo(MOVE_FORWARD_KEY);
        protected KeyInfo MoveBackward { get; set; } = new KeyInfo(MOVE_BACKWARD_KEY);
        protected KeyInfo MoveRight { get; set; } = new KeyInfo(MOVE_RIGHT_KEY);
        protected KeyInfo MoveLeft { get; set; } = new KeyInfo(MOVE_LEFT_KEY);
        protected KeyInfo MoveUp { get; set; } = new KeyInfo(MOVE_UP_KEY);
        protected KeyInfo MoveDown { get; set; } = new KeyInfo(MOVE_DOWN_KEY);
        protected KeyInfo RotateRight { get; set; } = new KeyInfo(ROTATE_RIGHT_KEY);
        protected KeyInfo RotateLeft { get; set; } = new KeyInfo(ROTATE_LEFT_KEY);
        protected KeyInfo TakeOff { get; set; } = new KeyInfo(TAKE_OFF_KEY);
        protected KeyInfo Land { get; set; } = new KeyInfo(LAND_KEY);

        public DroneCommand EvaluateKey(KeyInfo keyInfo)
        {
            return TryUpdateInputKey(keyInfo) ? EvaluateInputKeys() : null;
        }

        private bool TryUpdateInputKey(KeyInfo keyInfo)
        {
            try
            {
                // if "key" is not an input key -> InvalidOperationException, because GetInputKeys().Count(ki => ki.Key == key) == 0
                // if "key" is defined as an input key more than once -> InvalidOperationException, because 1 < GetInputKeys().Count(ki => ki.Key == key)
                KeyInfo keyInfoToUpdate = GetInputKeys().Single(ki => ki.Key == keyInfo.Key);
                if (keyInfoToUpdate.KeyState != keyInfo.KeyState)
                {
                    keyInfoToUpdate = keyInfo;
                    return true;
                }
                
                return false;
            }
            catch
            {
                return false;
            }
        }

        protected virtual DroneCommand EvaluateInputKeys()
        {
            if (TakeOff.KeyState == KeyState.Down)
            {
                return new TakeOffCommand();
            }
            else if (Land.KeyState == KeyState.Down)
            {
                return new LandCommand();
            }
            else
            {
                MoveCommand moveCommand = new MoveCommand();

                if (MoveForward.KeyState == KeyState.Down)
                {
                    moveCommand.Longitudinal += SIGN_VALUE;
                }
                if (MoveBackward.KeyState == KeyState.Down)
                {
                    moveCommand.Longitudinal -= SIGN_VALUE;
                }
                if (MoveRight.KeyState == KeyState.Down)
                {
                    moveCommand.Lateral += SIGN_VALUE;
                }
                if (MoveLeft.KeyState == KeyState.Down)
                {
                    moveCommand.Lateral -= SIGN_VALUE;
                }
                if (MoveUp.KeyState == KeyState.Down)
                {
                    moveCommand.Vertical += SIGN_VALUE;
                }
                if (MoveDown.KeyState == KeyState.Down)
                {
                    moveCommand.Vertical -= SIGN_VALUE;
                }
                if (RotateRight.KeyState == KeyState.Down)
                {
                    moveCommand.Yaw += SIGN_VALUE;
                }
                if (RotateLeft.KeyState == KeyState.Down)
                {
                    moveCommand.Yaw -= SIGN_VALUE;
                }

                return moveCommand;
            }
        }

        protected virtual IEnumerable<KeyInfo> GetInputKeys()
        {
            return typeof(GeneralDroneKeyInputEvaluator)
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(pi => pi.PropertyType == typeof(KeyInfo))
                .Select(pi => (KeyInfo)pi.GetValue(this));
        }
    }
}
