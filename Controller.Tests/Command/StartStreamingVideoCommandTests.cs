using Caduhd.Drone.Command;
using Xunit;

namespace Caduhd.Controller.Tests.Command
{
    public class StartStreamingVideoCommandTests
    {
        private StartStreamingVideoCommand _startStreamingVideoCommand;

        public StartStreamingVideoCommandTests()
        {
            _startStreamingVideoCommand = new StartStreamingVideoCommand();
        }

        [Fact]
        public void Copy_OriginalAndCopy_DifferentReferences()
        {
            var copy = _startStreamingVideoCommand.Copy();
            Assert.NotSame(_startStreamingVideoCommand, copy);
        }

        [Fact]
        public void Copy_OriginalAndCopy_SameType()
        {
            var copy = _startStreamingVideoCommand.Copy();
            Assert.IsType<StartStreamingVideoCommand>(copy);
        }

        [Fact]
        public void Equals_ComparandIsNull_ReturnsFalse()
        {
            Assert.False(_startStreamingVideoCommand.Equals(null));
        }

        [Fact]
        public void Equals_ComparandIsStartStreamingVideoCommand_ReturnsTrue()
        {
            var comparand = new StartStreamingVideoCommand();
            Assert.True(_startStreamingVideoCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsStartStreamingVideoCommandWithCameraCommandReference_ReturnsTrue()
        {
            CameraCommand comparand = new StartStreamingVideoCommand();
            Assert.True(_startStreamingVideoCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsStartStreamingVideoCommandWithDroneCommandReference_ReturnsTrue()
        {
            DroneCommand comparand = new StartStreamingVideoCommand();
            Assert.True(_startStreamingVideoCommand.Equals(comparand));
        }
    }
}
