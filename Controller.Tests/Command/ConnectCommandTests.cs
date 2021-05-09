using Caduhd.Controller.Command;
using Xunit;

namespace Caduhd.Controller.Tests.Command
{
    public class ConnectCommandTests
    {
        private ConnectCommand _connectCommand;

        public ConnectCommandTests()
        {
            _connectCommand = new ConnectCommand();
        }

        [Fact]
        public void Copy_OriginalAndCopy_DifferentReferences()
        {
            var copy = _connectCommand.Copy();
            Assert.NotSame(_connectCommand, copy);
        }

        [Fact]
        public void Copy_OriginalAndCopy_SameType()
        {
            var copy = _connectCommand.Copy();
            Assert.IsType<ConnectCommand>(copy);
        }

        [Fact]
        public void Equals_ComparandIsNull_ReturnsFalse()
        {
            Assert.False(_connectCommand.Equals(null));
        }

        [Fact]
        public void Equals_ComparandIsConnectCommand_ReturnsTrue()
        {
            var comparand = new ConnectCommand();
            Assert.True(_connectCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsConnectCommandWithControlCommandReference_ReturnsTrue()
        {
            ControlCommand comparand = new ConnectCommand();
            Assert.True(_connectCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsConnectCommandWithDroneCommandReference_ReturnsTrue()
        {
            DroneCommand comparand = new ConnectCommand();
            Assert.True(_connectCommand.Equals(comparand));
        }
    }
}
