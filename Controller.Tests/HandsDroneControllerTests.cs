using Caduhd.Controller.InputEvaluator;
using Caduhd.Drone;
using Caduhd.Drone.Command;
using Caduhd.HandsDetector;
using Caduhd.Input.Keyboard;
using Moq;
using System.Windows.Input;
using Xunit;

namespace Caduhd.Controller.Tests
{
    public class HandsDroneControllerTests
    {
        private readonly Mock<AbstractDrone> _controllableDroneMock;
        private readonly Mock<IDroneControllerKeyInputEvaluator> _keyInputEvaluatorMock;
        private readonly Mock<IDroneControllerHandsInputEvaluator> _handsInputEvaluatorMock;
        private readonly HandsDroneController _droneController;

        private readonly NormalizedHands _hands;

        public HandsDroneControllerTests()
        {
            _controllableDroneMock = new Mock<AbstractDrone>();
            _keyInputEvaluatorMock = new Mock<IDroneControllerKeyInputEvaluator>();
            _handsInputEvaluatorMock = new Mock<IDroneControllerHandsInputEvaluator>();
            _droneController = new HandsDroneController(_controllableDroneMock.Object,
                _handsInputEvaluatorMock.Object, _keyInputEvaluatorMock.Object);

            _hands = new NormalizedHands(new NormalizedHand(), new NormalizedHand());
        }

        [Fact]
        public void ProcessHandsInput_MoveCommandSentToDrone()
        {
            var moveCommand = new MoveCommand(1, 1, 1, 1);

            _handsInputEvaluatorMock
                .Setup(hie => hie.EvaluateHands(_hands))
                .Returns(moveCommand);

            _droneController.ProcessHandsInput(_hands);

            _controllableDroneMock
                .Verify(d => d.Control(moveCommand), Times.Once);
            _controllableDroneMock
                .Verify(d => d.Control(It.IsAny<DroneCommand>()), Times.Once);
        }

        [Fact]
        public void ProcessHandsInput_CommandNotSentAgainIfEqualsPreviousOne()
        {
            var moveCommand = new MoveCommand(1, 1, 1, 1);

            _handsInputEvaluatorMock
                .Setup(hie => hie.EvaluateHands(_hands))
                .Returns(moveCommand);

            _droneController.ProcessHandsInput(_hands);
            _droneController.ProcessHandsInput(_hands);

            _controllableDroneMock
                .Verify(d => d.Control(moveCommand), Times.Once);
            _controllableDroneMock
                .Verify(d => d.Control(It.IsAny<DroneCommand>()), Times.Once);
        }

        [Fact]
        public void ProcessHandsInput_FirstTakeOffCommandSentFromKeyboardThenMoveCommandSentFromHands_BothCommandsSentToDrone()
        {
            var takeOffKeyInfo = new KeyInfo(Key.Enter, KeyState.Down);
            var takeOffCommand = new TakeOffCommand();
            var moveCommand = new MoveCommand(1, 1, 1, 1);

            _keyInputEvaluatorMock
               .Setup(kie => kie.EvaluateKey(takeOffKeyInfo))
               .Returns(takeOffCommand);

            _handsInputEvaluatorMock
                .Setup(hie => hie.EvaluateHands(_hands))
                .Returns(moveCommand);

            _droneController.ProcessKeyInput(takeOffKeyInfo);
            _droneController.ProcessHandsInput(_hands);

            _controllableDroneMock
               .Verify(d => d.Control(takeOffCommand), Times.Once);
            _controllableDroneMock
                .Verify(d => d.Control(moveCommand), Times.Once);
            _controllableDroneMock
                .Verify(d => d.Control(It.IsAny<DroneCommand>()), Times.Exactly(2));
        }

        [Fact]
        public void ProcessHandsInput_FirstNotIdleMovingCommandSentFromKeyboardThenMoveCommandSentFromHands_OnlyTheCommandFromKeyboardSentToDrone()
        {
            var verticalUpKeyInfo = new KeyInfo(Key.W, KeyState.Down);
            var keyboardMoveCommand = new MoveCommand(0, 0, 1, 0);
            var handsMoveCommand = new MoveCommand(1, 1, 1, 1);

            _keyInputEvaluatorMock
               .Setup(kie => kie.EvaluateKey(verticalUpKeyInfo))
               .Returns(keyboardMoveCommand);

            _handsInputEvaluatorMock
                .Setup(hie => hie.EvaluateHands(_hands))
                .Returns(handsMoveCommand);

            _droneController.ProcessKeyInput(verticalUpKeyInfo);
            _droneController.ProcessHandsInput(_hands);

            _controllableDroneMock
               .Verify(d => d.Control(keyboardMoveCommand), Times.Once);
            _controllableDroneMock
                .Verify(d => d.Control(handsMoveCommand), Times.Never);
            _controllableDroneMock
                .Verify(d => d.Control(It.IsAny<DroneCommand>()), Times.Once);
        }

        [Fact]
        public void ProcessHandsInput_FirstIdleMoveCommandSentFromKeyboardThenMoveCommandFromHands_BothCommandsSentToDrone()
        {
            var keyboardIdleMoveCommand = MoveCommand.Idle;
            var handsMoveCommand = new MoveCommand(1, 1, 1, 1);

            _keyInputEvaluatorMock
               .Setup(kie => kie.EvaluateKey(It.IsAny<KeyInfo>()))
               .Returns(keyboardIdleMoveCommand);

            _handsInputEvaluatorMock
                .Setup(hie => hie.EvaluateHands(_hands))
                .Returns(handsMoveCommand);

            _droneController.ProcessKeyInput(new KeyInfo(Key.W, KeyState.Up));
            _droneController.ProcessHandsInput(_hands);

            _controllableDroneMock
               .Verify(d => d.Control(keyboardIdleMoveCommand), Times.Once);
            _controllableDroneMock
                .Verify(d => d.Control(handsMoveCommand), Times.Once);
            _controllableDroneMock
                .Verify(d => d.Control(It.IsAny<DroneCommand>()), Times.Exactly(2));
        }
    }
}
