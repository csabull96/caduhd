using Caduhd.Controller.Commands;

namespace Caduhd.Controller
{
    public interface IControllableDrone
    {
        void Control(DroneCommand droneCommant);
    }
}
