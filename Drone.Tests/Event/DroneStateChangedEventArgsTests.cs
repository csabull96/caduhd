namespace Caduhd.Drone.Tests.Event
{
    using Caduhd.Drone.Event;
    using Xunit;

    public class DroneStateChangedEventArgsTests
    {
        [Fact]
        public void DroneStateGetter_ReturnesTheDroneStateThatWasPassedToTheConstructor()
        {
            var droneState = new DroneState();
            var droneStateChangedEventArgs = new DroneStateChangedEventArgs(droneState);
            Assert.Same(droneState, droneStateChangedEventArgs.DroneState);
        }
    }
}
