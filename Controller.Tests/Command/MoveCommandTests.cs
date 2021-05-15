namespace Caduhd.Controller.Tests.Command
{
    using Caduhd.Drone.Command;
    using Xunit;

    public class MoveCommandTests
    {
        private const int LATERAL = 1;
        private const int LONGITUDINAL = 0;
        private const int VERTICAL = -1;
        private const int YAW = 0;

        private readonly MoveCommand moveCommandFromParameterlessConstructor;
        private readonly MoveCommand moveCommandFromParameterizedConstructor;

        public MoveCommandTests()
        {
            this.moveCommandFromParameterlessConstructor = new MoveCommand();
            this.moveCommandFromParameterizedConstructor = new MoveCommand(LATERAL, LONGITUDINAL, VERTICAL, YAW);
        }

        [Fact]
        public void LateralGetter_ParameterlessConstructor_ReturnsZero()
        {
            Assert.Equal(0, this.moveCommandFromParameterlessConstructor.Lateral);
        }

        [Fact]
        public void LateralGetter_ParameterizedConstructor_ReturnsValueSetThroughConstructor()
        {
            Assert.Equal(LATERAL, this.moveCommandFromParameterizedConstructor.Lateral);
        }

        [Fact]
        public void LongitudinalGetter_ParameterlessConstructor_ReturnsZero()
        {
            Assert.Equal(0, this.moveCommandFromParameterlessConstructor.Longitudinal);
        }

        [Fact]
        public void LongitudinalGetter_ParameterizedConstructor_ReturnsValueSetThroughConstructor()
        {
            Assert.Equal(LONGITUDINAL, this.moveCommandFromParameterizedConstructor.Longitudinal);
        }

        [Fact]
        public void VerticalGetter_ParameterlessConstructor_ReturnsZero()
        {
            Assert.Equal(0, this.moveCommandFromParameterlessConstructor.Vertical);
        }

        [Fact]
        public void VerticalGetter_ParameterizedConstructor_ReturnsValueSetThroughConstructor()
        {
            Assert.Equal(VERTICAL, this.moveCommandFromParameterizedConstructor.Vertical);
        }

        [Fact]
        public void YawGetter_ParameterlessConstructor_ReturnsZero()
        {
            Assert.Equal(0, this.moveCommandFromParameterlessConstructor.Yaw);
        }

        [Fact]
        public void YawGetter_ParameterizedConstructor_ReturnsValueSetThroughConstructor()
        {
            Assert.Equal(YAW, this.moveCommandFromParameterizedConstructor.Yaw);
        }

        [Fact]
        public void IdleGetter_ReturnsNeutralMovementCommand()
        {
            var neutral = MoveCommand.Idle;

            Assert.Equal(0, neutral.Lateral);
            Assert.Equal(0, neutral.Longitudinal);
            Assert.Equal(0, neutral.Vertical);
            Assert.Equal(0, neutral.Yaw);
        }

        [Fact]
        public void StillGetter_OnIdleMovement_ReturnsTrue()
        {
            Assert.True(MoveCommand.Idle.Still);
        }

        [Fact]
        public void StillGetter_OnActiveMovement_ReturnsFalse()
        {
            Assert.False(this.moveCommandFromParameterizedConstructor.Still);
        }

        [Fact]
        public void MovingGetter_OnIdleMovement_ReturnsFalse()
        {
            Assert.False(MoveCommand.Idle.Moving);
        }

        [Fact]
        public void MovingGetter_OnActiveMovement_ReturnsTrue()
        {
            Assert.True(this.moveCommandFromParameterizedConstructor.Moving);
        }

        [Fact]
        public void Constructor_Parameterized_ParametersThatAreSmallerThanMinOrGreaterThenMaxAreAdjusted()
        {
            var active = new MoveCommand(60, -13, 1, -2);

            Assert.Equal(1, active.Lateral);
            Assert.Equal(-1, active.Longitudinal);
            Assert.Equal(1, active.Vertical);
            Assert.Equal(-1, active.Yaw);
        }

        [Fact]
        public void Copy_OriginalAndCopy_DifferentReferences()
        {
            var copy = this.moveCommandFromParameterizedConstructor.Copy();
            Assert.NotSame(this.moveCommandFromParameterizedConstructor, copy);
        }

        [Fact]
        public void Copy_OriginalAndCopy_SameType()
        {
            var copy = this.moveCommandFromParameterizedConstructor.Copy();
            Assert.IsType<MoveCommand>(copy);
        }

        [Fact]
        public void Equals_ComparandIsNull_ReturnsFalse()
        {
            Assert.False(this.moveCommandFromParameterizedConstructor.Equals(null));
        }

        [Fact]
        public void Equals_ComparandIsMoveCommand_ReturnsTrue()
        {
            var comparand = new MoveCommand(LATERAL, LONGITUDINAL, VERTICAL, YAW);
            Assert.True(this.moveCommandFromParameterizedConstructor.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsMoveCommandWithMovementCommandReference_ReturnsTrue()
        {
            MovementCommand comparand = new MoveCommand(LATERAL, LONGITUDINAL, VERTICAL, YAW);
            Assert.True(this.moveCommandFromParameterizedConstructor.Equals(comparand));
        }

        [Fact]
        public void Equals_ComparandIsMoveCommandWithDroneCommandReference_ReturnsTrue()
        {
            DroneCommand comparand = new MoveCommand(LATERAL, LONGITUDINAL, VERTICAL, YAW);
            Assert.True(this.moveCommandFromParameterizedConstructor.Equals(comparand));
        }
    }
}
