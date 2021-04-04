using Caduhd.Controller;
using Caduhd.Controller.InputEvaluator;

namespace Caduhd.Drone
{
    public class DroneControllerKeyInputEvaluatorFactory
    {
        public IDroneKeyInputEvaluator GetDroneControllerKeyInputEvaluator(IControllableDrone drone)
        {
            if (drone is Tello)
            {
                return new TelloKeyInputEvaluator();
            }

            return new GeneralDroneKeyInputEvaluator();
        }
    }
}
