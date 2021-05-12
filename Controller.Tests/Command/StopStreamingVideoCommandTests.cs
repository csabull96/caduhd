using Caduhd.Drone.Command;
using Xunit;

namespace Caduhd.Controller.Tests.Command
{
    public class StopStreamingVideoCommandTests
    {
        private StopStreamingVideoCommand _stopStreamingVideoCommand;

        public StopStreamingVideoCommandTests()
        {
            _stopStreamingVideoCommand = new StopStreamingVideoCommand();
        }

        [Fact]
        public void Copy_OriginalAndCopy_DifferentReferences()
        {
            var copy = _stopStreamingVideoCommand.Copy();
            Assert.NotSame(_stopStreamingVideoCommand, copy);
        }

        [Fact]
        public void Copy_OriginalAndCopy_SameType()
        {
            var copy = _stopStreamingVideoCommand.Copy();
            Assert.IsType<StopStreamingVideoCommand>(copy);
        }

        [Fact]
        public void Equals_ComparandIsNull_ReturnsFalse()
        {
            Assert.False(_stopStreamingVideoCommand.Equals(null));
        }

        [Fact]
        public void Equals_ComparandIsStopStreamingVideoCommand_ReturnsTrue()
        {
            var comparand = new StopStreamingVideoCommand();
            Assert.True(_stopStreamingVideoCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsStopStreamingVideoCommandWithCameraCommandReference_ReturnsTrue()
        {
            CameraCommand comparand = new StopStreamingVideoCommand();
            Assert.True(_stopStreamingVideoCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsStopStreamingVideoCommandWithDroneCommandReference_ReturnsTrue()
        {
            DroneCommand comparand = new StopStreamingVideoCommand();
            Assert.True(_stopStreamingVideoCommand.Equals(comparand));
        }
    }
}
