using Caduhd.Controller.Command;

namespace Caduhd.Controller
{
    public abstract class AbstractDroneController
    {
        protected readonly IControllableDrone _drone;

        private DroneCommand _lastCommandSentToDrone;

        public AbstractDroneController(IControllableDrone drone)
        {
            _drone = drone;
            _lastCommandSentToDrone = null;
        }

        protected abstract void Control();

        public void Connect() => InternalControl(new ConnectCommand());

        public void StartStreamingVideo() => InternalControl(new StartStreamingVideoCommand());

        public void StopStreamingVideo() => InternalControl(new StopStreamingVideoCommand());

        protected void InternalControl(DroneCommand droneCommand)
        {
            if (_lastCommandSentToDrone == null ||
                droneCommand != null && !_lastCommandSentToDrone.Equals(droneCommand))
            {
                _lastCommandSentToDrone = droneCommand.Copy();
                _drone.Control(droneCommand);
            }
        }
    }
}
