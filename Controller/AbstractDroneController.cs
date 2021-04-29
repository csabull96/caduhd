using Caduhd.Controller.Command;

namespace Caduhd.Controller
{
    public abstract class AbstractDroneController
    {
        protected readonly IControllableDrone _drone;

        public AbstractDroneController(IControllableDrone drone)
        {
            _drone = drone;
        }

        public abstract void Control();

        protected void InternalControl(DroneCommand droneCommand) => _drone.Control(droneCommand);

        public void Connect() => _drone.Control(new ConnectCommand());

        public void StartStreamingVideo() => _drone.Control(new StartStreamingVideoCommand());

        public void StopStreamingVideo() => _drone.Control(new StopStreamingVideoCommand());
    }
}
