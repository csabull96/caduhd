namespace Caduhd.Controller.Command
{
    /// <summary>
    /// Start streaming video drone command.
    /// </summary>
    public sealed class StartStreamingVideoCommand : CameraCommand
    {
        /// <summary>
        /// Gets a copy of this <see cref="StartStreamingVideoCommand"/>.
        /// </summary>
        /// <returns>The copy of this <see cref="StartStreamingVideoCommand"/> as a <see cref="DroneCommand"/>.</returns>
        public override DroneCommand Copy() => new StartStreamingVideoCommand();

        /// <summary>
        /// The overriden Equals method.
        /// </summary>
        /// <param name="obj">The comparand.</param>
        /// <returns>True if this <see cref="StartStreamingVideoCommand"/> and the provided <paramref name="obj"/> are equal.</returns>
        public override bool Equals(object obj) => obj is StartStreamingVideoCommand;
    }
}
