namespace Caduhd.Controller.InputEvaluator
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;
    using Caduhd.Drone.Command;
    using Caduhd.Input.Keyboard;

    /// <summary>
    /// General drone key input evaluator.
    /// </summary>
    public class GeneralDroneKeyInputEvaluator : AbstractDroneInputEvaluator, IDroneControllerKeyInputEvaluator
    {
        private const Key MOVE_CONNECT_KEY = Key.D0;
        private const Key MOVE_FORWARD_KEY = Key.Up;
        private const Key MOVE_BACKWARD_KEY = Key.Down;
        private const Key MOVE_LEFT_KEY = Key.Left;
        private const Key MOVE_RIGHT_KEY = Key.Right;
        private const Key MOVE_UP_KEY = Key.W;
        private const Key MOVE_DOWN_KEY = Key.S;
        private const Key YAW_LEFT_KEY = Key.A;
        private const Key YAW_RIGHT_KEY = Key.D;
        private const Key TAKE_OFF_KEY = Key.Enter;
        private const Key LAND_KEY = Key.Space;

        /// <summary>
        /// Gets the <see cref="KeyInfo"/> of the connect key.
        /// </summary>
        protected KeyInfo Connect { get; private set; } = new KeyInfo(MOVE_CONNECT_KEY);

        /// <summary>
        /// Gets the <see cref="KeyInfo"/> of the forward movement key.
        /// </summary>
        protected KeyInfo MoveForward { get; private set; } = new KeyInfo(MOVE_FORWARD_KEY);

        /// <summary>
        /// Gets the <see cref="KeyInfo"/> of the backward movement key.
        /// </summary>
        protected KeyInfo MoveBackward { get; private set; } = new KeyInfo(MOVE_BACKWARD_KEY);

        /// <summary>
        /// Gets the <see cref="KeyInfo"/> of the left movement key.
        /// </summary>
        protected KeyInfo MoveLeft { get; private set; } = new KeyInfo(MOVE_LEFT_KEY);

        /// <summary>
        /// Gets the <see cref="KeyInfo"/> of the right movement key.
        /// </summary>
        protected KeyInfo MoveRight { get; private set; } = new KeyInfo(MOVE_RIGHT_KEY);

        /// <summary>
        /// Gets the <see cref="KeyInfo"/> of the upward movement key.
        /// </summary>
        protected KeyInfo MoveUp { get; private set; } = new KeyInfo(MOVE_UP_KEY);

        /// <summary>
        /// Gets the <see cref="KeyInfo"/> of the downward movement key.
        /// </summary>
        protected KeyInfo MoveDown { get; private set; } = new KeyInfo(MOVE_DOWN_KEY);

        /// <summary>
        /// Gets the <see cref="KeyInfo"/> of the yaw left movement key.
        /// </summary>
        protected KeyInfo YawLeft { get; private set; } = new KeyInfo(YAW_LEFT_KEY);

        /// <summary>
        /// Gets the <see cref="KeyInfo"/> of the yaw right movement key.
        /// </summary>
        protected KeyInfo YawRight { get; private set; } = new KeyInfo(YAW_RIGHT_KEY);

        /// <summary>
        /// Gets the <see cref="KeyInfo"/> of the take off movement key.
        /// </summary>
        protected KeyInfo TakeOff { get; private set; } = new KeyInfo(TAKE_OFF_KEY);

        /// <summary>
        /// Gets the <see cref="KeyInfo"/> of the land movement key.
        /// </summary>
        protected KeyInfo Land { get; private set; } = new KeyInfo(LAND_KEY);

        /// <summary>
        /// Evaluates the input <see cref="KeyInfo"/>.
        /// </summary>
        /// <param name="keyInfo"><see cref="KeyInfo"/> to evaluate.</param>
        /// <returns>The evaluated <paramref name="keyUpdated"/> as a <see cref="DroneCommand"/>. If key is not supported then it returns null.</returns>
        public DroneCommand EvaluateKey(KeyInfo keyInfo)
        {
            return this.TryUpdateInputKey(keyInfo) ? this.EvaluateInputKeys(keyInfo) : null;
        }

        /// <summary>
        /// Evaluates the general drone input keys together.
        /// </summary>
        /// <param name="keyUpdated">The updated <see cref="KeyInfo"/>.</param>
        /// <returns>The evaluated <see cref="KeyInfo"/> as a <see cref="DroneCommand"/>.</returns>
        protected virtual DroneCommand EvaluateInputKeys(KeyInfo keyUpdated)
        {
            if (this.Connect.KeyState == KeyState.Down)
            {
                return new ConnectCommand();
            }
            else if (this.TakeOff.KeyState == KeyState.Down)
            {
                return new TakeOffCommand();
            }
            else if (this.Land.KeyState == KeyState.Down)
            {
                return new LandCommand();
            }
            else
            {
                MoveCommand moveCommand = new MoveCommand();

                if (this.MoveForward.KeyState == KeyState.Down)
                {
                    moveCommand.Longitudinal += MOVE_FORWARD;
                }

                if (this.MoveBackward.KeyState == KeyState.Down)
                {
                    moveCommand.Longitudinal += MOVE_BACKWARD;
                }

                if (this.MoveLeft.KeyState == KeyState.Down)
                {
                    moveCommand.Lateral += MOVE_LEFT;
                }

                if (this.MoveRight.KeyState == KeyState.Down)
                {
                    moveCommand.Lateral += MOVE_RIGHT;
                }

                if (this.MoveUp.KeyState == KeyState.Down)
                {
                    moveCommand.Vertical += MOVE_UP;
                }

                if (this.MoveDown.KeyState == KeyState.Down)
                {
                    moveCommand.Vertical += MOVE_DOWN;
                }

                if (this.YawLeft.KeyState == KeyState.Down)
                {
                    moveCommand.Yaw += YAW_LEFT;
                }

                if (this.YawRight.KeyState == KeyState.Down)
                {
                    moveCommand.Yaw += YAW_RIGHT;
                }

                return moveCommand;
            }
        }

        /// <summary>
        /// This is a method to collect every <see cref="KeyInfo"/> objects from the properties on this instance of the class.
        /// </summary>
        /// <returns>Returns the <see cref="KeyInfo"/> property values on this instance.</returns>
        protected virtual IEnumerable<KeyInfo> GetInputKeys()
        {
            return typeof(GeneralDroneKeyInputEvaluator)
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(pi => pi.PropertyType == typeof(KeyInfo))
                .Select(pi => (KeyInfo)pi.GetValue(this));
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
                    keyInfoToUpdate.KeyState = keyInfo.KeyState;
                    return true;
                }
                
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
