﻿namespace Caduhd.Controller.Tests.InputEvaluator
{
    using System.Windows.Input;
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Drone.Command;
    using Caduhd.Input.Keyboard;
    using Xunit;

    public class GeneralDroneKeyInputEvaluatorTests
    {
        private readonly IDroneControllerKeyInputEvaluator generalKeyInputEvaluator;

        public GeneralDroneKeyInputEvaluatorTests()
        {
            this.generalKeyInputEvaluator = new GeneralDroneKeyInputEvaluator();
        }

        [Fact]
        public void EvaluateKey_EnterDown_EvaluatedAsTakeOffCommand()
        {
            var expectedCommand = new TakeOffCommand();
            var takeOffKeyInfo = new KeyInfo(Key.Enter, KeyState.Down);
            var commandEvaluated = this.generalKeyInputEvaluator.EvaluateKey(takeOffKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_SpaceDown_EvaluatedAsLandCommand()
        {
            var expectedCommand = new LandCommand();
            var landKeyInfo = new KeyInfo(Key.Space, KeyState.Down);
            var commandEvaluated = this.generalKeyInputEvaluator.EvaluateKey(landKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_UpArrowDown_EvaluatedAsMoveForward()
        {
            var expectedCommand = new MoveCommand(0, 1, 0, 0);
            var moveForwardKeyInfo = new KeyInfo(Key.Up, KeyState.Down);
            var commandEvaluated = this.generalKeyInputEvaluator.EvaluateKey(moveForwardKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_DownArrowDown_EvaluatedAsMoveBackward()
        {
            var expectedCommand = new MoveCommand(0, -1, 0, 0);
            var moveBackwardKeyInfo = new KeyInfo(Key.Down, KeyState.Down);
            var commandEvaluated = this.generalKeyInputEvaluator.EvaluateKey(moveBackwardKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_LeftArrowDown_EvaluatedAsMoveLeft()
        {
            var expectedCommand = new MoveCommand(-1, 0, 0, 0);
            var moveLeftKeyInfo = new KeyInfo(Key.Left, KeyState.Down);
            var commandEvaluated = this.generalKeyInputEvaluator.EvaluateKey(moveLeftKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_RightArrowDown_EvaluatedAsMoveRight()
        {
            var expectedCommand = new MoveCommand(1, 0, 0, 0);
            var moveRightKeyInfo = new KeyInfo(Key.Right, KeyState.Down);
            var commandEvaluated = this.generalKeyInputEvaluator.EvaluateKey(moveRightKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_WDown_EvaluatedAsMoveUpwards()
        {
            var expectedCommand = new MoveCommand(0, 0, 1, 0);
            var moveUpwardsKeyInfo = new KeyInfo(Key.W, KeyState.Down);
            var commandEvaluated = this.generalKeyInputEvaluator.EvaluateKey(moveUpwardsKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_SDown_EvaluatedAsMoveDownwards()
        {
            var expectedCommand = new MoveCommand(0, 0, -1, 0);
            var moveDownwardsKeyInfo = new KeyInfo(Key.S, KeyState.Down);
            var commandEvaluated = this.generalKeyInputEvaluator.EvaluateKey(moveDownwardsKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_ADown_EvaluatedAsYawLeft()
        {
            var expectedCommand = new MoveCommand(0, 0, 0, -1);
            var yawLeftKeyInfo = new KeyInfo(Key.A, KeyState.Down);
            var commandEvaluated = this.generalKeyInputEvaluator.EvaluateKey(yawLeftKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Fact]
        public void EvaluateKey_DDown_EvaluatedAsYawRight()
        {
            var expectedCommand = new MoveCommand(0, 0, 0, 1);
            var yawRightKeyInfo = new KeyInfo(Key.D, KeyState.Down);
            var commandEvaluated = this.generalKeyInputEvaluator.EvaluateKey(yawRightKeyInfo);
            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Theory]
        [InlineData(Key.Left, Key.Up, Key.W, Key.D, -1, 1, 1, 1)]
        [InlineData(Key.Right, Key.Down, Key.S, Key.A, 1, -1, -1, -1)]
        [InlineData(Key.Up, Key.Down, Key.S, Key.D, 0, 0, -1, 1)]
        public void EvaluateKey_MultipleMoveKeysDown_EvaluatedCorrectly(Key first, Key second, Key third, Key fourth, int lateral, int longitudinal, int veritcal, int yaw)
        {
            var expectedCommand = new MoveCommand(lateral, longitudinal, veritcal, yaw);

            var firstKeyInfo = new KeyInfo(first, KeyState.Down);
            var secondKeyInfo = new KeyInfo(second, KeyState.Down);
            var thirdKeyInfo = new KeyInfo(third, KeyState.Down);
            var fourthKeyInfo = new KeyInfo(fourth, KeyState.Down);

            this.generalKeyInputEvaluator.EvaluateKey(secondKeyInfo);
            this.generalKeyInputEvaluator.EvaluateKey(firstKeyInfo);
            this.generalKeyInputEvaluator.EvaluateKey(thirdKeyInfo);
            var commandEvaluated = this.generalKeyInputEvaluator.EvaluateKey(fourthKeyInfo);

            Assert.Equal(expectedCommand, commandEvaluated);
        }

        [Theory]
        [InlineData(Key.LeftShift)]
        [InlineData(Key.RightShift)]
        [InlineData(Key.Back)]
        [InlineData(Key.Q)]
        [InlineData(Key.E)]
        public void EvaluateKey_NotSupportedKey_EvaluatedAsNull(Key key)
        {
            var notSupportedKeyInfo = new KeyInfo(key, KeyState.Down);
            var commandEvaluated = this.generalKeyInputEvaluator.EvaluateKey(notSupportedKeyInfo);
            Assert.Null(commandEvaluated);
        }
    }
}
