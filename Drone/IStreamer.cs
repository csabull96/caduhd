namespace Caduhd.Drone
{
    using Caduhd.Drone.Event;

    /// <summary>
    /// New drone video frame event handler.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="args">The event args containing the new frame.</param>
    public delegate void NewDroneVideoFrameEventHandler(object source, NewDroneCameraFrameEventArgs args);

    /// <summary>
    /// The streamer interface.
    /// </summary>
    public interface IStreamer
    {
        /// <summary>
        /// The event that fires when a new drone camera frama has arrived.
        /// </summary>
        event NewDroneVideoFrameEventHandler NewCameraFrame;
    }
}
