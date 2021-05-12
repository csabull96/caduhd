using Caduhd.Drone.Command;
using Xunit;

namespace Caduhd.Controller.Tests.Command
{
    public sealed class LandCommandTests
    {
        private LandCommand _landCommand;

        public LandCommandTests()
        {
            _landCommand = new LandCommand();
        }

        [Fact]
        public void Copy_OriginalAndCopy_DifferentReferences()
        {
            var copy = _landCommand.Copy();
            Assert.NotSame(_landCommand, copy);
        }

        [Fact]
        public void Copy_OriginalAndCopy_SameType()
        {
            var copy = _landCommand.Copy();
            Assert.IsType<LandCommand>(copy);
        }

        [Fact]
        public void Equals_ComparandIsNull_ReturnsFalse()
        {
            Assert.False(_landCommand.Equals(null));
        }

        [Fact]
        public void Equals_ComparandIsLandCommand_ReturnsTrue()
        {
            var comparand = new LandCommand();
            Assert.True(_landCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsLandCommandWithMovementCommandReference_ReturnsTrue()
        {
            MovementCommand comparand = new LandCommand();
            Assert.True(_landCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsLandCommandWithDroneCommandReference_ReturnsTrue()
        {
            DroneCommand comparand = new LandCommand();
            Assert.True(_landCommand.Equals(comparand));
        }
    }
}
