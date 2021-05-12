namespace Caduhd.Drone.Command
{
    /// <summary>
    /// Connect drone command.
    /// </summary>
    public sealed class ConnectCommand : ControlCommand
    {
        /// <summary>
        /// Gets a copy of this <see cref="ConnectCommand"/>.
        /// </summary>
        /// <returns>The copy of this <see cref="ConnectCommand"/> as a <see cref="DroneCommand"/>.</returns>
        public override DroneCommand Copy() => new ConnectCommand();

        /// <summary>
        /// The overriden Equals method.
        /// </summary>
        /// <param name="obj">The comparand.</param>
        /// <returns>True if this <see cref="ConnectCommand"/> and the provided <paramref name="obj"/> are equal.</returns>
        public override bool Equals(object obj) => obj is ConnectCommand;
    }
}
