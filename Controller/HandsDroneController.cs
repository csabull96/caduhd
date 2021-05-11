namespace Caduhd.Controller
{
    using Caduhd.Controller.Command;
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.HandsDetector;

    /// <summary>
    /// A drone controller that is used to control a <see cref="IControllableDrone"/> with hands and the keyboard.
    /// </summary>
    public class HandsDroneController : KeyboardDroneController, IHandsInputHandler
    {
        private readonly IDroneHandsInputEvaluator handsInputEvaluator;

        /// <summary>
        /// Gets or sets the result of the latest hands input evaluation as <see cref="MoveCommand"/>.
        /// </summary>
        protected MoveCommand LatestHandsInputEvaluated { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandsDroneController"/> class.
        /// </summary>
        /// <param name="drone">The <see cref="IControllableDrone"/> drone that we would like to control with this controller.</param>
        /// <param name="handsInputEvaluator">The desired implementation of the <see cref="IDroneHandsInputEvaluator"/> interface.</param>
        /// <param name="keyInputEvaluator">The desired implementation of the <see cref="IDroneKeyInputEvaluator"/> interface.</param>
        public HandsDroneController(IControllableDrone drone, IDroneHandsInputEvaluator handsInputEvaluator, IDroneKeyInputEvaluator keyInputEvaluator)
            : base(drone, keyInputEvaluator)
        {
            this.handsInputEvaluator = handsInputEvaluator;
        }

        /// <summary>
        /// Processes a hands input.
        /// </summary>
        /// <param name="hands">The key input to evaluate.</param>
        /// <returns>The evaluation result of the <paramref name="hands"/>.</returns>
        public InputProcessResult ProcessHandsInput(NormalizedHands hands)
        {
            this.LatestHandsInputEvaluated = this.handsInputEvaluator.EvaluateHands(hands);
            DroneControllerHandsInputProcessResult result =
                new DroneControllerHandsInputProcessResult(this.LatestHandsInputEvaluated.Copy() as MoveCommand);
            this.Control();
            return result;
        }

        /// <summary>
        /// Evaluated latest inputs together.
        /// </summary>
        protected override void Control()
        {
            DroneCommand inputsEvaluated = this.EvaluateInputs();
            this.InternalControl(inputsEvaluated);
        }

        private DroneCommand EvaluateInputs()
        {
            // key input has always priority over the hands input
            if (this.LatestKeyInputEvaluated != null)
            {
                DroneCommand latestKeyInputEvaluatedCopy = this.LatestKeyInputEvaluated.Copy();

                // If the evaluated input from the keyboard was executed, then it has to be set to null,
                // otherwise the evaluated hands input will never have the chance to get executed.
                // After being evaluated the _latestKeyInputEvaluated is always set to null
                // unless it's a MoveCommand which represents a moving state.
                // Why?
                // Imagine the following scenario:
                // The hand detector is enabled but we want to control the drone using the keyboard.               
                // We are pressing the left key arrow (nothing else).
                // We are going to enter the KeyboardDroneController.ProcessKeyInput method only once.
                // Right after the key is evaluated the _latestKeyInputEvaluated is set to null.
                // At the same time because of the enabled hand detector we're receiving several input per second from the web camera.
                // This means that even though the left arrow key is still held down, instead of the _latestKeyInputEvaluated
                // the _latestHandsInputEvaluated is going to be executed.
                // (Because the _latestKeyInputEvaluated was set to null before.)
                if (!(this.LatestKeyInputEvaluated is MoveCommand moveCommand) || moveCommand.Still)
                {
                    this.LatestKeyInputEvaluated = null;
                }

                return latestKeyInputEvaluatedCopy;
            }
            else if (this.LatestHandsInputEvaluated != null)
            {
                DroneCommand latestHandsInputEvaluatedCopy = this.LatestHandsInputEvaluated.Copy();
                return latestHandsInputEvaluatedCopy;
            }

            return null;
        }
    }
}