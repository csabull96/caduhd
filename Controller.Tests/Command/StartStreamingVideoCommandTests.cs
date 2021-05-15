namespace Caduhd.Controller.Tests.Command
{
    using Caduhd.Drone.Command;
    using Xunit;

    public class StartStreamingVideoCommandTests
    {
        private StartStreamingVideoCommand startStreamingVideoCommand;

        public StartStreamingVideoCommandTests()
        {
            this.startStreamingVideoCommand = new StartStreamingVideoCommand();
        }

        [Fact]
        public void Copy_OriginalAndCopy_DifferentReferences()
        {
            var copy = this.startStreamingVideoCommand.Copy();
            Assert.NotSame(this.startStreamingVideoCommand, copy);
        }

        [Fact]
        public void Copy_OriginalAndCopy_SameType()
        {
            var copy = this.startStreamingVideoCommand.Copy();
            Assert.IsType<StartStreamingVideoCommand>(copy);
        }

        [Fact]
        public void Equals_ComparandIsNull_ReturnsFalse()
        {
            Assert.False(this.startStreamingVideoCommand.Equals(null));
        }

        [Fact]
        public void Equals_ComparandIsStartStreamingVideoCommand_ReturnsTrue()
        {
            var comparand = new StartStreamingVideoCommand();
            Assert.True(this.startStreamingVideoCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsStartStreamingVideoCommandWithCameraCommandReference_ReturnsTrue()
        {
            CameraCommand comparand = new StartStreamingVideoCommand();
            Assert.True(this.startStreamingVideoCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsStartStreamingVideoCommandWithDroneCommandReference_ReturnsTrue()
        {
            DroneCommand comparand = new StartStreamingVideoCommand();
            Assert.True(this.startStreamingVideoCommand.Equals(comparand));
        }
    }
}
