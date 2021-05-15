namespace Caduhd.Drone
{
    using System;
    using Caduhd.Drone.Command;

    /// <summary>
    /// The abstract drone class, the base class for any drone.
    /// </summary>
    public abstract class AbstractDrone : IDisposable
    {
        /// <summary>
        /// The interface to speak to control the drone.
        /// </summary>
        /// <param name="droneCommand">The <see cref="DroneCommand"/> to be executed by the drone.</param>
        public abstract void Control(DroneCommand droneCommand);

        /// <summary>
        /// Disposing this instance.
        /// </summary>
        public abstract void Dispose();
    }
}
