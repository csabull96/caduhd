namespace Caduhd.Controller
{
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Drone;
    using Caduhd.Drone.Command;
    using Caduhd.Input.Keyboard;

    /// <summary>
    /// A drone controller that is used to control a <see cref="AbstractDrone"/> with the keyboard.
    /// </summary>
    public class KeyboardDroneController : AbstractDroneController, IKeyInputHandler
    {
        /// <summary>
        /// The desired implementation of the <see cref="IDroneControllerKeyInputEvaluator"/> interface.
        /// </summary>
        private readonly IDroneControllerKeyInputEvaluator keyInputEvaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardDroneController"/> class.
        /// </summary>
        /// <param name="drone">The <see cref="AbstractDrone"/> drone that we would like to control with this controller.</param>
        /// <param name="keyInputEvaluator">The desired implementation of the <see cref="IDroneControllerKeyInputEvaluator"/> interface.</param>
        public KeyboardDroneController(AbstractDrone drone, IDroneControllerKeyInputEvaluator keyInputEvaluator)
            : base(drone)
        {
            this.keyInputEvaluator = keyInputEvaluator;
        }

        /// <summary>
        /// Gets or sets the result of the latest key input evaluation as <see cref="DroneCommand"/>.
        /// </summary>
        protected DroneCommand LatestKeyInputEvaluated { get; set; }

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