using Caduhd.Drone.Event;
using Xunit;

namespace Caduhd.Drone.Tests.Event
{
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
