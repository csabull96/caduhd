using Caduhd.Controller.Commands;

namespace Caduhd.Controller
{
    public abstract class AbstractDroneController
    {
        protected readonly IControllableDrone m_drone;

        public AbstractDroneController(IControllableDrone drone)
        {
            m_drone = drone;
        }

        // no input parameter
        // behaviour should be based on private class members
        public abstract void Control();

        protected void InternalControl(DroneCommand droneCommand)
        {
            m_drone.Control(droneCommand);
        }

        public void Connect()
        {
            m_drone.Control(new ConnectCommand());
        }

        public void TakeOff()
        {
            m_drone.Control(new TakeOffCommand());
        }

        public void Land()
        {
            m_drone.Control(new LandCommand());
        }

        public void StartStreamingVideo()
        {
            m_drone.Control(new StartStreamingVideoCommand());
        }

        public void StopStreamingVideo()
        {
            m_drone.Control(new StopStreamingVideoCommand());
        }
    }
}
