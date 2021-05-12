using Caduhd.Drone.Command;
using Xunit;

namespace Caduhd.Controller.Tests.Command
{
    public class DisconnectCommandTests
    {
        private DisconnectCommand _disconnectCommand;

        public DisconnectCommandTests()
        {
            _disconnectCommand = new DisconnectCommand();
        }

        [Fact]
        public void Copy_OriginalAndCopy_DifferentReferences()
        {
            var copy = _disconnectCommand.Copy();
            Assert.NotSame(_disconnectCommand, copy);
        }

        [Fact]
        public void Copy_OriginalAndCopy_SameType()
        {
            var copy = _disconnectCommand.Copy();
            Assert.IsType<DisconnectCommand>(copy);
        }

        [Fact]
        public void Equals_ComparandIsNull_ReturnsFalse()
        {
            Assert.False(_disconnectCommand.Equals(null));
        }

        [Fact]
        public void Equals_ComparandIsDisconnectCommand_ReturnsTrue()
        {
            var comparand = new DisconnectCommand();
            Assert.True(_disconnectCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsDisconnectCommandWithControlCommandReference_ReturnsTrue()
        {
            ControlCommand comparand = new DisconnectCommand();
            Assert.True(_disconnectCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsDisconnectCommandWithDroneCommandReference_ReturnsTrue()
        {
            DroneCommand comparand = new DisconnectCommand();
            Assert.True(_disconnectCommand.Equals(comparand));
        }
    }
}
