using Caduhd.Controller;
using Caduhd.Controller.InputEvaluator;
using Caduhd.Drone.Dji;
using System;

namespace Caduhd.Drone
{
    public class DroneControllerKeyInputEvaluatorFactory
    {
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
