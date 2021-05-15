namespace Caduhd.Controller.Tests.Command
{
    using Caduhd.Drone.Command;
    using Xunit;

    public class ConnectCommandTests
    {
        private ConnectCommand connectCommand;

        public ConnectCommandTests()
        {
            this.connectCommand = new ConnectCommand();
        }

        [Fact]
        public void Copy_OriginalAndCopy_DifferentReferences()
        {
            var copy = this.connectCommand.Copy();
            Assert.NotSame(this.connectCommand, copy);
        }

        [Fact]
        public void Copy_OriginalAndCopy_SameType()
        {
            var copy = this.connectCommand.Copy();
            Assert.IsType<ConnectCommand>(copy);
        }

        [Fact]
        public void Equals_ComparandIsNull_ReturnsFalse()
        {
            Assert.False(this.connectCommand.Equals(null));
        }

        [Fact]
        public void Equals_ComparandIsConnectCommand_ReturnsTrue()
        {
            var comparand = new ConnectCommand();
            Assert.True(this.connectCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsConnectCommandWithControlCommandReference_ReturnsTrue()
        {
            ControlCommand comparand = new ConnectCommand();
            Assert.True(this.connectCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsConnectCommandWithDroneCommandReference_ReturnsTrue()
        {
            DroneCommand comparand = new ConnectCommand();
            Assert.True(this.connectCommand.Equals(comparand));
        }
    }
}
