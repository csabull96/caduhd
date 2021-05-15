namespace Caduhd.Controller.Tests.InputEvaluator
{
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Drone.Command;
    using Caduhd.HandsDetector;
    using Xunit;

    public class DroneHandsInputEvaluatorTests
    {
        private readonly DroneControllerHandsInputEvaluator handsInputEvaluator;
        private readonly NormalizedHands neutralHands;

        public DroneHandsInputEvaluatorTests()
        {
            var left = new NormalizedHand(0.15, 0.5, 0.15);
            var right = new NormalizedHand(0.85, 0.5, 0.15);
            this.neutralHands = new NormalizedHands(left, right);
            this.handsInputEvaluator = new DroneControllerHandsInputEvaluator();
            this.handsInputEvaluator.Tune(this.neutralHands);
        }

        [Fact]
        public void EvaluateHands_NeutralHands_NoMovement()
        {
            Assert.Equal(MoveCommand.Idle, this.handsInputEvaluator.EvaluateHands(this.neutralHands));
        }

        [Fact]
        public void EvaluateHands_LeftHandUpRightHandDown_MoveLeft()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(1, 0, 0, 0);

            var left = new NormalizedHand(0.15, 0.2, 0.15);
            var right = new NormalizedHand(0.85, 0.7, 0.15);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, this.handsInputEvaluator.EvaluateHands(hands));
        }

        [Fact]
        public void EvaluateHands_LeftHandDownRightHandUp_MoveRight()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(-1, 0, 0, 0);

            var left = new NormalizedHand(0.15, 0.7, 0.15);
            var right = new NormalizedHand(0.85, 0.2, 0.15);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, this.handsInputEvaluator.EvaluateHands(hands));
        }

        [Fact]
        public void EvaluateHands_BothHandsInForeground_MoveForward()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(0, 1, 0, 0);

            var left = new NormalizedHand(0.15, 0.5, 0.25);
            var right = new NormalizedHand(0.85, 0.5, 0.25);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, this.handsInputEvaluator.EvaluateHands(hands));
        }

        [Fact]
        public void EvaluateHands_BothHandsInBackground_MoveBackward()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(0, -1, 0, 0);

            var left = new NormalizedHand(0.15, 0.5, 0.08);
            var right = new NormalizedHand(0.85, 0.5, 0.08);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, this.handsInputEvaluator.EvaluateHands(hands));
        }

        [Fact]
        public void EvaluateHands_BothHandsUp_MoveUpwards()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(0, 0, 1, 0);

            var left = new NormalizedHand(0.15, 0.2, 0.15);
            var right = new NormalizedHand(0.85, 0.2, 0.15);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, this.handsInputEvaluator.EvaluateHands(hands));
        }

        [Fact]
        public void EvaluateHands_BothHandsDown_MoveDownwards()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(0, 0, -1, 0);

            var left = new NormalizedHand(0.15, 0.8, 0.15);
            var right = new NormalizedHand(0.85, 0.8, 0.15);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, this.handsInputEvaluator.EvaluateHands(hands));
        }

        [Fact]
        public void EvaluateHands_LeftHandForegroundRightHandBackground_YawRight()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(0, 0, 0, 1);

            var left = new NormalizedHand(0.15, 0.5, 0.25);
            var right = new NormalizedHand(0.85, 0.5, 0.08);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, this.handsInputEvaluator.EvaluateHands(hands));
        }

        [Fact]
        public void EvaluateHands_LeftHandBackgroundRightHandForeground_YawLeft()
        {
            MoveCommand expectedMoveCommand = new MoveCommand(0, 0, 0, -1);

            var left = new NormalizedHand(0.15, 0.5, 0.08);
            var right = new NormalizedHand(0.85, 0.5, 0.25);
            var hands = new NormalizedHands(left, right);

            Assert.Equal(expectedMoveCommand, this.handsInputEvaluator.EvaluateHands(hands));
        }
    }
}
