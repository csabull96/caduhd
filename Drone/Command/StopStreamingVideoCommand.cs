namespace Caduhd.Drone.Command
{
    /// <summary>
    /// Stop streaming video command.
    /// </summary>
    public sealed class StopStreamingVideoCommand : CameraCommand
    {
        /// <summary>
        /// Gets a copy of this <see cref="StopStreamingVideoCommand"/>.
        /// </summary>
        /// <returns>The copy of this <see cref="StopStreamingVideoCommand"/> as a <see cref="DroneCommand"/>.</returns>
        public override DroneCommand Copy() => new StopStreamingVideoCommand();

        /// <summary>
        /// The overriden Equals method.
        /// </summary>
        /// <param name="obj">The comparand.</param>
        /// <returns>True if this <see cref="StopStreamingVideoCommand"/> and the provided <paramref name="obj"/> are equal.</returns>
        public override bool Equals(object obj) => obj is StopStreamingVideoCommand;
    }
}
