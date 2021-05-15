namespace Caduhd.Controller.InputEvaluator
{
    using Caduhd.Drone.Command;

    /// <summary>
    /// Drone controller key input process result.
    /// </summary>
    public class DroneControllerKeyInputProcessResult : InputProcessResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DroneControllerKeyInputProcessResult"/> class.
        /// </summary>
        /// <param name="result">The <see cref="DroneCommand"/> that was evaluated from the hands input.</param>
        public DroneControllerKeyInputProcessResult(DroneCommand result)
        {
            this.Result = result;
        }

        /// <summary>
        /// Gets the evaluated key input as <see cref="DroneCommand"/>.
        /// </summary>
        public DroneCommand Result { get; private set; }
    }
}