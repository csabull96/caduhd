namespace Caduhd.Drone.Command
{
    /// <summary>
    /// Land drone command.
    /// </summary>
    public sealed class LandCommand : MovementCommand
    {
        /// <summary>
        /// Gets a copy of this <see cref="LandCommand"/>.
        /// </summary>
        /// <returns>The copy of this <see cref="LandCommand"/> as a <see cref="DroneCommand"/>.</returns>
        public override DroneCommand Copy() => new LandCommand();

        /// <summary>
        /// The overriden Equals method.
        /// </summary>
        /// <param name="obj">The comparand.</param>
        /// <returns>True if this <see cref="LandCommand"/> and the provided <paramref name="obj"/> are equal.</returns>
        public override bool Equals(object obj) => obj is LandCommand;
    }
}
