using Caduhd.Common;
using Caduhd.Drone.Event;
using System.Drawing;
using Xunit;

namespace Caduhd.Drone.Tests.Event
{
    public class NewDroneCameraFrameEventArgsTests
    {
        [Fact]
        public void DroneStateGetter_ReturnesTheDroneStateThatWasPassedToTheConstructor()
        {
            var bgrImage = BgrImage.GetBlank(100, 100, Color.White);
            var newDroneCameraFrameEventArgs = new NewDroneCameraFrameEventArgs(bgrImage);
            Assert.Same(bgrImage, newDroneCameraFrameEventArgs.Frame);
        }
    }
}
