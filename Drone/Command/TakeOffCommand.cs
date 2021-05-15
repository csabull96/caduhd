namespace Caduhd.Drone.Command
{
    /// <summary>
    /// Take off command.
    /// </summary>
    public sealed class TakeOffCommand : MovementCommand
    {
        /// <summary>
        /// Gets a copy of this <see cref="TakeOffCommand"/>.
        /// </summary>
        /// <returns>The copy of this <see cref="TakeOffCommand"/> as a <see cref="DroneCommand"/>.</returns>
        public override DroneCommand Copy() => new TakeOffCommand();

        /// <summary>
        /// The overriden Equals method.
        /// </summary>
        /// <param name="obj">The comparand.</param>
        /// <returns>True if this <see cref="TakeOffCommand"/> and the provided <paramref name="obj"/> are equal.</returns>
        public override bool Equals(object obj) => obj is TakeOffCommand;

        /// <summary>
        /// The overriden <see cref="GetHashCode"/> method.
        /// </summary>
        /// <returns>The hascode of this instance.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
