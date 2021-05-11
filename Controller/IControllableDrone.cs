namespace Caduhd.Controller
{
    using Caduhd.Controller.Command;

    /// <summary>
    /// <see cref="IControllableDrone"/> interface.
    /// </summary>
    public interface IControllableDrone
    {
        /// <summary>
        /// Sends the requested command for execution to the drone.
        /// </summary>
        /// <param name="droneCommant">The requested <see cref="DroneCommand"/> object.</param>
        void Control(DroneCommand droneCommant);
    }
}
