using Caduhd.Drone.Command;
using Xunit;

namespace Caduhd.Controller.Tests.Command
{
    public class TakeOffCommandTests
    {
        private TakeOffCommand _takeOffCommand;

        public TakeOffCommandTests()
        {
            _takeOffCommand = new TakeOffCommand();
        }

        [Fact]
        public void Copy_OriginalAndCopy_DifferentReferences()
        {
            var copy = _takeOffCommand.Copy();
            Assert.NotSame(_takeOffCommand, copy);
        }

        [Fact]
        public void Copy_OriginalAndCopy_SameType()
        {
            var copy = _takeOffCommand.Copy();
            Assert.IsType<TakeOffCommand>(copy);
        }

        [Fact]
        public void Equals_ComparandIsNull_ReturnsFalse()
        {
            Assert.False(_takeOffCommand.Equals(null));
        }

        [Fact]
        public void Equals_ComparandIsTakeOffCommand_ReturnsTrue()
        {
            var comparand = new TakeOffCommand();
            Assert.True(_takeOffCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsTakeOffCommandWithMovementCommandReference_ReturnsTrue()
        {
            MovementCommand comparand = new TakeOffCommand();
            Assert.True(_takeOffCommand.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsTakeOffCommandWithDroneCommandReference_ReturnsTrue()
        {
            DroneCommand comparand = new TakeOffCommand();
            Assert.True(_takeOffCommand.Equals(comparand));
        }
    }
}
