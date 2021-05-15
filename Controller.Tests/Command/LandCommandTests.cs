namespace Caduhd.Controller.Tests.Command
{
    using Caduhd.Drone.Command;
    using Xunit;

    public sealed class LandCommandTests
    {
        private LandCommand landCommand;

        public LandCommandTests()
        {
            this.landCommand = new LandCommand();
        }

        [Fact]
        public void Copy_OriginalAndCopy_DifferentReferences()
        {
            var copy = this.landCommand.Copy();
            Assert.NotSame(this.landCommand, copy);
        }

        [Fact]
        public void Copy_OriginalAndCopy_SameType()
        {
            var copy = this.landCommand.Copy();
            Assert.IsType<LandCommand>(copy);
        }

        [Fact]
        public void Equals_ComparandIsNull_ReturnsFalse()
        {
            Assert.False(this.landCommand.Equals(null));
        }

        [Fact]
        public void Equals_ComparandIsLandCommand_ReturnsTrue()
        {
            var comparand = new LandCommand();
            Assert.True(this.landCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsLandCommandWithMovementCommandReference_ReturnsTrue()
        {
            MovementCommand comparand = new LandCommand();
            Assert.True(this.landCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsLandCommandWithDroneCommandReference_ReturnsTrue()
        {
            DroneCommand comparand = new LandCommand();
            Assert.True(this.landCommand.Equals(comparand));
        }
    }
}
