namespace Caduhd.Controller.Tests.Command
{
    using Caduhd.Drone.Command;
    using Xunit;

    public class StopStreamingVideoCommandTests
    {
        private readonly StopStreamingVideoCommand stopStreamingVideoCommand;

        public StopStreamingVideoCommandTests()
        {
            this.stopStreamingVideoCommand = new StopStreamingVideoCommand();
        }

        [Fact]
        public void Copy_OriginalAndCopy_DifferentReferences()
        {
            var copy = this.stopStreamingVideoCommand.Copy();
            Assert.NotSame(this.stopStreamingVideoCommand, copy);
        }

        [Fact]
        public void Copy_OriginalAndCopy_SameType()
        {
            var copy = this.stopStreamingVideoCommand.Copy();
            Assert.IsType<StopStreamingVideoCommand>(copy);
        }

        [Fact]
        public void Equals_ComparandIsNull_ReturnsFalse()
        {
            Assert.False(this.stopStreamingVideoCommand.Equals(null));
        }

        [Fact]
        public void Equals_ComparandIsStopStreamingVideoCommand_ReturnsTrue()
        {
            var comparand = new StopStreamingVideoCommand();
            Assert.True(this.stopStreamingVideoCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsStopStreamingVideoCommandWithCameraCommandReference_ReturnsTrue()
        {
            CameraCommand comparand = new StopStreamingVideoCommand();
            Assert.True(this.stopStreamingVideoCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsStopStreamingVideoCommandWithDroneCommandReference_ReturnsTrue()
        {
            DroneCommand comparand = new StopStreamingVideoCommand();
            Assert.True(this.stopStreamingVideoCommand.Equals(comparand));
        }
    }
}
