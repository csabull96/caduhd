namespace Caduhd.Controller
{
    using Caduhd.Controller.Command;
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Input.Keyboard;

    /// <summary>
    /// A drone controller that is used to control a <see cref="IControllableDrone"/> with the keyboard.
    /// </summary>
    public class KeyboardDroneController : AbstractDroneController, IKeyInputHandler
    {
        /// <summary>
        /// The desired implementation of the <see cref="IDroneKeyInputEvaluator"/> interface.
        /// </summary>
        private readonly IDroneKeyInputEvaluator keyInputEvaluator;

        /// <summary>
        /// Gets or sets the result of the latest key input evaluation as <see cref="DroneCommand"/>.
        /// </summary>
        protected DroneCommand LatestKeyInputEvaluated { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardDroneController"/> class.
        /// </summary>
        /// <param name="drone">The <see cref="IControllableDrone"/> drone that we would like to control with this controller.</param>
        /// <param name="keyInputEvaluator">The desired implementation of the <see cref="IDroneKeyInputEvaluator"/> interface.</param>
        public KeyboardDroneController(IControllableDrone drone, IDroneKeyInputEvaluator keyInputEvaluator)
            : base(drone)
        {
            this.keyInputEvaluator = keyInputEvaluator;
        }

        /// <summary>
        /// Processes a key input.
        /// </summary>
        /// <param name="keyInfo">The key input to evaluate.</param>
        /// <returns>The evaluation result of the <paramref name="keyInfo"/>.</returns>
        public InputProcessResult ProcessKeyInput(KeyInfo keyInfo)
        {
            DroneCommand keyEvaluated = this.keyInputEvaluator.EvaluateKey(keyInfo);

            if (keyEvaluated == null)
            {
                return null;
            }

            this.LatestKeyInputEvaluated = keyEvaluated;
            DroneControllerKeyInputProcessResult result =
                new DroneControllerKeyInputProcessResult(this.LatestKeyInputEvaluated.Copy());
            this.Control();
            return result;
        }

        /// <summary>
        /// Evaluated latest inputs together.
        /// </summary>
        protected override void Control()
        {
            this.InternalControl(this.LatestKeyInputEvaluated);
        }
    }
}