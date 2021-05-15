namespace Caduhd.Controller.Tests
{
    using System.Windows.Input;
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Drone;
    using Caduhd.Drone.Command;
    using Caduhd.HandsDetector;
    using Caduhd.Input.Keyboard;
    using Moq;
    using Xunit;

    public class HandsDroneControllerTests
    {
        private readonly Mock<AbstractDrone> controllableDroneMock;
        private readonly Mock<IDroneControllerKeyInputEvaluator> keyInputEvaluatorMock;
        private readonly Mock<IDroneControllerHandsInputEvaluator> handsInputEvaluatorMock;
        private readonly HandsDroneController droneController;

        private readonly NormalizedHands hands;

        public HandsDroneControllerTests()
        {
            this.controllableDroneMock = new Mock<AbstractDrone>();
            this.keyInputEvaluatorMock = new Mock<IDroneControllerKeyInputEvaluator>();
            this.handsInputEvaluatorMock = new Mock<IDroneControllerHandsInputEvaluator>();
            this.droneController = new HandsDroneController(this.controllableDroneMock.Object,
                this.handsInputEvaluatorMock.Object,
                this.keyInputEvaluatorMock.Object);

            this.hands = new NormalizedHands(new NormalizedHand(), new NormalizedHand());
        }

        [Fact]
        public void ProcessHandsInput_MoveCommandSentToDrone()
        {
            var moveCommand = new MoveCommand(1, 1, 1, 1);

            this.handsInputEvaluatorMock
                .Setup(hie => hie.EvaluateHands(this.hands))
                .Returns(moveCommand);

            this.droneController.ProcessHandsInput(this.hands);

            this.controllableDroneMock
                .Verify(d => d.Control(moveCommand), Times.Once);
            this.controllableDroneMock
                .Verify(d => d.Control(It.IsAny<DroneCommand>()), Times.Once);
        }

        [Fact]
        public void ProcessHandsInput_CommandNotSentAgainIfEqualsPreviousOne()
        {
            var moveCommand = new MoveCommand(1, 1, 1, 1);

            this.handsInputEvaluatorMock
                .Setup(hie => hie.EvaluateHands(this.hands))
                .Returns(moveCommand);

            this.droneController.ProcessHandsInput(this.hands);
            this.droneController.ProcessHandsInput(this.hands);

            this.controllableDroneMock
                .Verify(d => d.Control(moveCommand), Times.Once);
            this.controllableDroneMock
                .Verify(d => d.Control(It.IsAny<DroneCommand>()), Times.Once);
        }

        [Fact]
        public void ProcessHandsInput_FirstTakeOffCommandSentFromKeyboardThenMoveCommandSentFromHands_BothCommandsSentToDrone()
        {
            var takeOffKeyInfo = new KeyInfo(Key.Enter, KeyState.Down);
            var takeOffCommand = new TakeOffCommand();
            var moveCommand = new MoveCommand(1, 1, 1, 1);

            this.keyInputEvaluatorMock
               .Setup(kie => kie.EvaluateKey(takeOffKeyInfo))
               .Returns(takeOffCommand);

            this.handsInputEvaluatorMock
                .Setup(hie => hie.EvaluateHands(this.hands))
                .Returns(moveCommand);

            this.droneController.ProcessKeyInput(takeOffKeyInfo);
            this.droneController.ProcessHandsInput(this.hands);

            this.controllableDroneMock
               .Verify(d => d.Control(takeOffCommand), Times.Once);
            this.controllableDroneMock
                .Verify(d => d.Control(moveCommand), Times.Once);
            this.controllableDroneMock
                .Verify(d => d.Control(It.IsAny<DroneCommand>()), Times.Exactly(2));
        }

        [Fact]
        public void ProcessHandsInput_FirstNotIdleMovingCommandSentFromKeyboardThenMoveCommandSentFromHands_OnlyTheCommandFromKeyboardSentToDrone()
        {
            var verticalUpKeyInfo = new KeyInfo(Key.W, KeyState.Down);
            var keyboardMoveCommand = new MoveCommand(0, 0, 1, 0);
            var handsMoveCommand = new MoveCommand(1, 1, 1, 1);

            this.keyInputEvaluatorMock
               .Setup(kie => kie.EvaluateKey(verticalUpKeyInfo))
               .Returns(keyboardMoveCommand);

            this.handsInputEvaluatorMock
                .Setup(hie => hie.EvaluateHands(this.hands))
                .Returns(handsMoveCommand);

            this.droneController.ProcessKeyInput(verticalUpKeyInfo);
            this.droneController.ProcessHandsInput(this.hands);

            this.controllableDroneMock
               .Verify(d => d.Control(keyboardMoveCommand), Times.Once);
            this.controllableDroneMock
                .Verify(d => d.Control(handsMoveCommand), Times.Never);
            this.controllableDroneMock
                .Verify(d => d.Control(It.IsAny<DroneCommand>()), Times.Once);
        }

        [Fact]
        public void ProcessHandsInput_FirstIdleMoveCommandSentFromKeyboardThenMoveCommandFromHands_BothCommandsSentToDrone()
        {
            var keyboardIdleMoveCommand = MoveCommand.Idle;
            var handsMoveCommand = new MoveCommand(1, 1, 1, 1);

            this.keyInputEvaluatorMock
               .Setup(kie => kie.EvaluateKey(It.IsAny<KeyInfo>()))
               .Returns(keyboardIdleMoveCommand);

            this.handsInputEvaluatorMock
                .Setup(hie => hie.EvaluateHands(this.hands))
                .Returns(handsMoveCommand);

            this.droneController.ProcessKeyInput(new KeyInfo(Key.W, KeyState.Up));
            this.droneController.ProcessHandsInput(this.hands);

            this.controllableDroneMock
               .Verify(d => d.Control(keyboardIdleMoveCommand), Times.Once);
            this.controllableDroneMock
                .Verify(d => d.Control(handsMoveCommand), Times.Once);
            this.controllableDroneMock
                .Verify(d => d.Control(It.IsAny<DroneCommand>()), Times.Exactly(2));
        }
    }
}
