namespace Caduhd.Controller.InputEvaluator
{
    using Caduhd.Drone.Command;

    /// <summary>
    /// Drone controller hands input process result.
    /// </summary>
    public class DroneControllerHandsInputProcessResult : InputProcessResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DroneControllerHandsInputProcessResult"/> class.
        /// </summary>
        /// <param name="result">The <see cref="MoveCommand"/> that was evaluated from the hands input.</param>
        public DroneControllerHandsInputProcessResult(MoveCommand result)
        {
            this.Result = result;
        }

        /// <summary>
        /// Gets the evaluated hands input as <see cref="MoveCommand"/>.
        /// </summary>
        public MoveCommand Result { get; private set; }
    }
}