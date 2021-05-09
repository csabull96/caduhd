using Caduhd.Controller.Command;
using Caduhd.Controller.InputEvaluator;
using Caduhd.HandsDetector;
using Xunit;

namespace Caduhd.Controller.Tests.InputEvaluator
{
    public class DroneHandsInputEvaluatorTests
    {
        private DroneHandsInputEvaluator _handsInputEvaluator;
        private NormalizedHands _neutralHands;
        public DroneHandsInputEvaluatorTests()
        {
            var left = new NormalizedHand(0.15, 0.5, 0.15);
            var right = new NormalizedHand(0.85, 0.5, 0.15);
            _neutralHands = new NormalizedHands(left, right);
            _handsInputEvaluator = new DroneHandsInputEvaluator();
            _handsInputEvaluator.Tune(_neutralHands);
        }

        [Fact]
        public void EvaluateHands_NeutralHands_NoMovement()
        {
            Assert.Equal(MoveCommand.Idle, _handsInputEvaluator.EvaluateHands(_neutralHands));
        }

        [Fact]
        public void EvaluateHands_LeftHandUpRightHandDown_MoveLeft()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(1, 0, 0, 0);

            var left = new NormalizedHand(0.15, 0.2, 0.15);
            var right = new NormalizedHand(0.85, 0.7, 0.15);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, _handsInputEvaluator.EvaluateHands(hands));
        }

        [Fact]
        public void EvaluateHands_LeftHandDownRightHandUp_MoveRight()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(-1, 0, 0, 0);

            var left = new NormalizedHand(0.15, 0.7, 0.15);
            var right = new NormalizedHand(0.85, 0.2, 0.15);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, _handsInputEvaluator.EvaluateHands(hands));
        }

        [Fact]
        public void EvaluateHands_BothHandsInForeground_MoveForward()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(0, 1, 0, 0);

            var left = new NormalizedHand(0.15, 0.5, 0.25);
            var right = new NormalizedHand(0.85, 0.5, 0.25);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, _handsInputEvaluator.EvaluateHands(hands));
        }

        [Fact]
        public void EvaluateHands_BothHandsInBackground_MoveBackward()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(0, -1, 0, 0);

            var left = new NormalizedHand(0.15, 0.5, 0.08);
            var right = new NormalizedHand(0.85, 0.5, 0.08);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, _handsInputEvaluator.EvaluateHands(hands));
        }

        [Fact]
        public void EvaluateHands_BothHandsUp_MoveUpwards()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(0, 0, 1, 0);

            var left = new NormalizedHand(0.15, 0.2, 0.15);
            var right = new NormalizedHand(0.85, 0.2, 0.15);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, _handsInputEvaluator.EvaluateHands(hands));
        }

        [Fact]
        public void EvaluateHands_BothHandsDown_MoveDownwards()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(0, 0, -1, 0);

            var left = new NormalizedHand(0.15, 0.8, 0.15);
            var right = new NormalizedHand(0.85, 0.8, 0.15);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, _handsInputEvaluator.EvaluateHands(hands));
        }

        [Fact]
        public void EvaluateHands_LeftHandForegroundRightHandBackground_YawRight()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(0, 0, 0, 1);

            var left = new NormalizedHand(0.15, 0.5, 0.25);
            var right = new NormalizedHand(0.85, 0.5, 0.08);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, _handsInputEvaluator.EvaluateHands(hands));
        }

        [Fact]
        public void EvaluateHands_LeftHandBackgroundRightHandForeground_YawLeft()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(0, 0, 0, -1);

            var left = new NormalizedHand(0.15, 0.5, 0.08);
            var right = new NormalizedHand(0.85, 0.5, 0.25);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, _handsInputEvaluator.EvaluateHands(hands));
        }
    }
}
