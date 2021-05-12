using Caduhd.Drone.Command;

namespace Caduhd.Controller.InputEvaluator
{

    /// <summary>
    /// Drone controller hands input process result.
    /// </summary>
    public class DroneControllerHandsInputProcessResult : InputProcessResult
    {
        /// <summary>
        /// Gets the evaluated hands input as <see cref="MoveCommand"/>.
        /// </summary>
        public MoveCommand Result { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DroneControllerHandsInputProcessResult"/> class.
        /// </summary>
        /// <param name="result">The <see cref="MoveCommand"/> that was evaluated from the hands input.</param>
        public DroneControllerHandsInputProcessResult(MoveCommand result)
        {
            this.Result = result;
        }
    }
}