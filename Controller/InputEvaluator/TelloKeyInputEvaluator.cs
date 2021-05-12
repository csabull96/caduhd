namespace Caduhd.Controller.InputEvaluator
{
    using Caduhd.Drone.Command;
    using Caduhd.Input.Keyboard;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;

    /// <summary>
    /// The specific key input evaluator for the Tello.
    /// </summary>
    public class TelloKeyInputEvaluator : GeneralDroneKeyInputEvaluator
    {
        private const Key START_STREAMING_VIDEO_KEY = Key.RightShift;
        private const Key STOP_STREAMING_VIDEO_KEY = Key.LeftShift;

        /// <summary>
        /// Gets the <see cref="KeyInfo"/> of the start streaming video key.
        /// </summary>
        protected KeyInfo StartStreamingVideo { get; private set; } = new KeyInfo(START_STREAMING_VIDEO_KEY);

        /// <summary>
        /// Gets the <see cref="KeyInfo"/> of the stop streaming video key.
        /// </summary>
        protected KeyInfo StopStreamingVideo { get; private set; } = new KeyInfo(STOP_STREAMING_VIDEO_KEY);

        /// <summary>
        /// Evaluates the Tello specific and general drone input keys together.
        /// </summary>
        /// <param name="keyUpdated">The updated <see cref="KeyInfo"/>.</param>
        /// <returns>The evaluated <paramref name="keyUpdated"/> as a <see cref="DroneCommand"/>.</returns>
        protected override DroneCommand EvaluateInputKeys(KeyInfo keyUpdated)
        {
            if (keyUpdated.Key == this.StartStreamingVideo.Key && keyUpdated.KeyState == KeyState.Down)
            {
                return new StartStreamingVideoCommand();
            }
            else if (keyUpdated.Key == this.StopStreamingVideo.Key && keyUpdated.KeyState == KeyState.Down)
            {
                return new StopStreamingVideoCommand();
            }
            else
            {
                return base.EvaluateInputKeys(keyUpdated);
            }
        }

        /// <summary>
        /// This is a method to collect every <see cref="KeyInfo"/> objects from the properties on this instance of the class.
        /// </summary>
        /// <returns>Returns the <see cref="KeyInfo"/> property values on this instance.</returns>
        protected override IEnumerable<KeyInfo> GetInputKeys()
        {
            return typeof(TelloKeyInputEvaluator)
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(pi => pi.PropertyType == typeof(KeyInfo))
                .Select(pi => (KeyInfo)pi.GetValue(this));
        }
    }
}
