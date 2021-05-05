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

        public void Connect() => InternalControl(new ConnectCommand());

        public void StartStreamingVideo() => InternalControl(new StartStreamingVideoCommand());

        public void StopStreamingVideo() => InternalControl(new StopStreamingVideoCommand());

        protected void InternalControl(DroneCommand droneCommand) => _drone.Control(droneCommand);
    }
}
