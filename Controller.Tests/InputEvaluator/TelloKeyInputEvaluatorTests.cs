namespace Caduhd.Controller.Tests.InputEvaluator
{
    using System.Windows.Input;
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Drone.Command;
    using Caduhd.Input.Keyboard;
    using Xunit;

    public class TelloKeyInputEvaluatorTests
    {
        private readonly IDroneControllerKeyInputEvaluator telloKeyInputEvaluator;

        public TelloKeyInputEvaluatorTests()
        {
            this.telloKeyInputEvaluator = new TelloKeyInputEvaluator();
        }

        [Fact]
        public void EvaluateKey_EnterDown_EvaluatedAsTakeOffCommand()
        {
            var expectedCommand = new TakeOffCommand();
            var takeOffKeyInfo = new KeyInfo(Key.Enter, KeyState.Down);
            var commandEvaluated = this.telloKeyInputEvaluator.EvaluateKey(takeOffKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_SpaceDown_EvaluatedAsLandCommand()
        {
            var expectedCommand = new LandCommand();
            var landKeyInfo = new KeyInfo(Key.Space, KeyState.Down);
            var commandEvaluated = this.telloKeyInputEvaluator.EvaluateKey(landKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_UpArrowDown_EvaluatedAsMoveForward()
        {
            var expectedCommand = new MoveCommand(0, 1, 0, 0);
            var moveForwardKeyInfo = new KeyInfo(Key.Up, KeyState.Down);
            var commandEvaluated = this.telloKeyInputEvaluator.EvaluateKey(moveForwardKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_DownArrowDown_EvaluatedAsMoveBackward()
        {
            var expectedCommand = new MoveCommand(0, -1, 0, 0);
            var moveBackwardKeyInfo = new KeyInfo(Key.Down, KeyState.Down);
            var commandEvaluated = this.telloKeyInputEvaluator.EvaluateKey(moveBackwardKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_LeftArrowDown_EvaluatedAsMoveLeft()
        {
            var expectedCommand = new MoveCommand(-1, 0, 0, 0);
            var moveLeftKeyInfo = new KeyInfo(Key.Left, KeyState.Down);
            var commandEvaluated = this.telloKeyInputEvaluator.EvaluateKey(moveLeftKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_RightArrowDown_EvaluatedAsMoveRight()
        {
            var expectedCommand = new MoveCommand(1, 0, 0, 0);
            var moveRightKeyInfo = new KeyInfo(Key.Right, KeyState.Down);
            var commandEvaluated = this.telloKeyInputEvaluator.EvaluateKey(moveRightKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_WDown_EvaluatedAsMoveUpwards()
        {
            var expectedCommand = new MoveCommand(0, 0, 1, 0);
            var moveUpwardsKeyInfo = new KeyInfo(Key.W, KeyState.Down);
            var commandEvaluated = this.telloKeyInputEvaluator.EvaluateKey(moveUpwardsKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_SDown_EvaluatedAsMoveDownwards()
        {
            var expectedCommand = new MoveCommand(0, 0, -1, 0);
            var moveDownwardsKeyInfo = new KeyInfo(Key.S, KeyState.Down);
            var commandEvaluated = this.telloKeyInputEvaluator.EvaluateKey(moveDownwardsKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_ADown_EvaluatedAsYawLeft()
        {
            var expectedCommand = new MoveCommand(0, 0, 0, -1);
            var yawLeftKeyInfo = new KeyInfo(Key.A, KeyState.Down);
            var commandEvaluated = this.telloKeyInputEvaluator.EvaluateKey(yawLeftKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_DDown_EvaluatedAsYawRight()
        {
            var expectedCommand = new MoveCommand(0, 0, 0, 1);
            var yawRightKeyInfo = new KeyInfo(Key.D, KeyState.Down);
            var commandEvaluated = this.telloKeyInputEvaluator.EvaluateKey(yawRightKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_RightShift_EvaluatedAsStartStreamingVideoCommand()
        {
            var expectedCommand = new StartStreamingVideoCommand();
            var startStreamingVideoKeyInfo = new KeyInfo(Key.RightShift, KeyState.Down);
            var commandEvaluated = this.telloKeyInputEvaluator.EvaluateKey(startStreamingVideoKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_LeftShift_EvaluatedAsStopStreamingVideoCommand()
        {
            var expectedCommand = new StopStreamingVideoCommand();
            var stopStreamingVideoKeyInfo = new KeyInfo(Key.LeftShift, KeyState.Down);
            var commandEvaluated = this.telloKeyInputEvaluator.EvaluateKey(stopStreamingVideoKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Theory]
        [InlineData(Key.Back)]
        [InlineData(Key.Q)]
        [InlineData(Key.E)]
        public void EvaluateKey_NotSupportedKey_EvaluatedAsNull(Key key)
        {
            var notSupportedKeyInfo = new KeyInfo(key, KeyState.Down);
            var commandEvaluated = this.telloKeyInputEvaluator.EvaluateKey(notSupportedKeyInfo);
            Assert.Null(commandEvaluated);
        }
    }
}
