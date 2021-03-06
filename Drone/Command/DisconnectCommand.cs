﻿namespace Caduhd.Drone.Command
{
    /// <summary>
    /// Disconnected drone command.
    /// </summary>
    public sealed class DisconnectCommand : ControlCommand
    {
        /// <summary>
        /// Gets a copy of this <see cref="DisconnectCommand"/>.
        /// </summary>
        /// <returns>The copy of this <see cref="DisconnectCommand"/> as a <see cref="DroneCommand"/>.</returns>
        public override DroneCommand Copy() => new DisconnectCommand();

        /// <summary>
        /// The overriden Equals method.
        /// </summary>
        /// <param name="obj">The comparand.</param>
        /// <returns>True if this <see cref="DisconnectCommand"/> and the provided <paramref name="obj"/> are equal.</returns>
        public override bool Equals(object obj) => obj is DisconnectCommand;

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
