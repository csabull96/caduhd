namespace Caduhd.Drone
{
    using Caduhd.Drone.Event;

    /// <summary>
    /// The drone state changed event handler.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="args">The event args containing the object that represents the drone's current state.</param>
    public delegate void DroneStateChangedEventHandler(object source, DroneStateChangedEventArgs args);

    /// <summary>
    /// The stateful interface.
    /// </summary>
    public interface IStateful
    {
        /// <summary>
        /// The event that fires when the lates state of the drone has arrived.
        /// </summary>
        event DroneStateChangedEventHandler StateChanged;
    }
}
