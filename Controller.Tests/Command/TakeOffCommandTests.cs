namespace Caduhd.Controller.Tests.Command
{
    using Caduhd.Drone.Command;
    using Xunit;

    public class TakeOffCommandTests
    {
        private TakeOffCommand takeOffCommand;

        public TakeOffCommandTests()
        {
            this.takeOffCommand = new TakeOffCommand();
        }

        [Fact]
        public void Copy_OriginalAndCopy_DifferentReferences()
        {
            var copy = this.takeOffCommand.Copy();
            Assert.NotSame(this.takeOffCommand, copy);
        }

        [Fact]
        public void Copy_OriginalAndCopy_SameType()
        {
            var copy = this.takeOffCommand.Copy();
            Assert.IsType<TakeOffCommand>(copy);
        }

        [Fact]
        public void Equals_ComparandIsNull_ReturnsFalse()
        {
            Assert.False(this.takeOffCommand.Equals(null));
        }

        [Fact]
        public void Equals_ComparandIsTakeOffCommand_ReturnsTrue()
        {
            var comparand = new TakeOffCommand();
            Assert.True(this.takeOffCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsTakeOffCommandWithMovementCommandReference_ReturnsTrue()
        {
            MovementCommand comparand = new TakeOffCommand();
            Assert.True(this.takeOffCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsTakeOffCommandWithDroneCommandReference_ReturnsTrue()
        {
            DroneCommand comparand = new TakeOffCommand();
            Assert.True(this.takeOffCommand.Equals(comparand));
        }
    }
}
