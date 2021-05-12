namespace Caduhd.Drone.Command
{
    /// <summary>
    /// The base class for every type of drone command.
    /// </summary>
    public abstract class DroneCommand
    {
        /// <summary>
        /// Gets a copy of this <see cref="DroneCommand"/>.
        /// </summary>
        /// <returns>The copy of this <see cref="DroneCommand"/> as a <see cref="DroneCommand"/>.</returns>
        public abstract DroneCommand Copy();
    }
}
