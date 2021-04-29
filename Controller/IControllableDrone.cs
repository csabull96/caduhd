using Caduhd.Controller.Command;

namespace Caduhd.Controller
{
    public interface IControllableDrone
    {
        void Control(DroneCommand droneCommant);
    }
}
