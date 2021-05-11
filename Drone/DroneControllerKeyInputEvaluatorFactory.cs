namespace Caduhd.Drone
{
    using System;
    using Caduhd.Controller;
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Drone.Dji;

    /// <summary>
    /// Drone controller key input evaluator factory.
    /// </summary>
    public class DroneControllerKeyInputEvaluatorFactory
    {
        /// <summary>
        /// Based on the type of the <see cref="IControllableDrone"/> gives the corresponding <see cref="IDroneKeyInputEvaluator"/> implementation.
        /// </summary>
        /// <param name="drone">The <see cref="IControllableDrone"/> implementation for which we want to get the corresponding <see cref="IDroneKeyInputEvaluator"/> implementation.</param>
        /// <returns>The <see cref="IDroneKeyInputEvaluator"/> implementation that corresponds to the <paramref name="drone"/>.</returns>
        public IDroneKeyInputEvaluator GetDroneControllerKeyInputEvaluator(IControllableDrone drone)
        {
            if (drone == null)
            {
                throw new ArgumentNullException("The IControllableDrone was null.");
            }

            if (drone is Tello)
            {
                return new TelloKeyInputEvaluator();
            }

            return new GeneralDroneKeyInputEvaluator();
        }
    }
}
