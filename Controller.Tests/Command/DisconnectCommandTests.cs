namespace Caduhd.Controller.Tests.Command
{
    using Caduhd.Drone.Command;
    using Xunit;

    public class DisconnectCommandTests
    {
        private DisconnectCommand disconnectCommand;

        public DisconnectCommandTests()
        {
            this.disconnectCommand = new DisconnectCommand();
        }

        [Fact]
        public void Copy_OriginalAndCopy_DifferentReferences()
        {
            var copy = this.disconnectCommand.Copy();
            Assert.NotSame(this.disconnectCommand, copy);
        }

        [Fact]
        public void Copy_OriginalAndCopy_SameType()
        {
            var copy = this.disconnectCommand.Copy();
            Assert.IsType<DisconnectCommand>(copy);
        }

        [Fact]
        public void Equals_ComparandIsNull_ReturnsFalse()
        {
            Assert.False(this.disconnectCommand.Equals(null));
        }

        [Fact]
        public void Equals_ComparandIsDisconnectCommand_ReturnsTrue()
        {
            var comparand = new DisconnectCommand();
            Assert.True(this.disconnectCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsDisconnectCommandWithControlCommandReference_ReturnsTrue()
        {
            ControlCommand comparand = new DisconnectCommand();
            Assert.True(this.disconnectCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsDisconnectCommandWithDroneCommandReference_ReturnsTrue()
        {
            DroneCommand comparand = new DisconnectCommand();
            Assert.True(this.disconnectCommand.Equals(comparand));
        }
    }
}
