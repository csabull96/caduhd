using Caduhd.Controller.InputEvaluator;
using Caduhd.Drone;
using Caduhd.Drone.Command;
using Caduhd.Input.Keyboard;
using Moq;
using System.Windows.Input;
using Xunit;

namespace Caduhd.Controller.Tests
{
    public class KeyboardDroneControllerTests
    {
        private readonly Mock<AbstractDrone> _controllableDroneMock;
        private readonly Mock<IDroneControllerKeyInputEvaluator> _keyInputEvaluatorMock;
        private readonly KeyboardDroneController _droneController;

        public KeyboardDroneControllerTests()
        {
            _controllableDroneMock = new Mock<AbstractDrone>();
            _keyInputEvaluatorMock = new Mock<IDroneControllerKeyInputEvaluator>();
            _droneController = new KeyboardDroneController(_controllableDroneMock.Object, _keyInputEvaluatorMock.Object);
        }

        [Fact]
        public void Connect_DroneControlCalledOnce()
        {
            _droneController.Connect();
            _controllableDroneMock.Verify(d => d.Control(It.IsAny<ConnectCommand>()), Times.Once);
        }

        [Fact]
        public void StartStreamingVideo_DroneControlCalledOnce()
        {
            _droneController.StartStreamingVideo();
            _controllableDroneMock.Verify(d => d.Control(It.IsAny<StartStreamingVideoCommand>()), Times.Once);
        }

        [Fact]
        public void StopStreamingVideo_DroneControlCalledOnce()
        {
            _droneController.StopStreamingVideo();
            _controllableDroneMock.Verify(d => d.Control(It.IsAny<StopStreamingVideoCommand>()), Times.Once);
        }

        [Fact]
        public void ProcessKeyInput_KeyIsSupported_DroneControlCalledOnce()
        {
            var moveCommand = new MoveCommand()
            {
                Lateral = 0,
                Longitudinal = 0,
                Vertical = -1,
                Yaw = 0
            };

            _keyInputEvaluatorMock
                .Setup(kie => kie.EvaluateKey(It.IsAny<KeyInfo>()))
                .Returns(moveCommand);

            _droneController.ProcessKeyInput(new KeyInfo(Key.W, KeyState.Down));

            _controllableDroneMock.Verify(d => d.Control(moveCommand), Times.Once);
        }

        [Fact]
        public void ProcessKeyInput_KeyIsSupported_ReturnsEvaluatedDroneCommand()
        {
            var moveCommand = new MoveCommand()
            {
                Lateral = 0,
                Longitudinal = 0,
                Vertical = -1,
                Yaw = 0
            };

            _keyInputEvaluatorMock
                .Setup(kie => kie.EvaluateKey(It.IsAny<KeyInfo>()))
                .Returns(moveCommand);

            var result = _droneController.ProcessKeyInput(new KeyInfo(Key.W, KeyState.Down));

            _controllableDroneMock.Verify(d => d.Control(moveCommand), Times.Once);

            Assert.IsType<DroneControllerKeyInputProcessResult>(result);
            DroneCommand resultDroneCommand = (result as DroneControllerKeyInputProcessResult).Result;
            MoveCommand resultMoveCommand = resultDroneCommand as MoveCommand;

            Assert.True(moveCommand.Equals(resultMoveCommand));
        }

        [Fact]
        public void ProcessKeyInput_KeyNotSupported_DroneControlNeverCalled()
        {
            MoveCommand moveCommand = null;

            _keyInputEvaluatorMock
                .Setup(kie => kie.EvaluateKey(It.IsAny<KeyInfo>()))
                .Returns(moveCommand);

            _droneController.ProcessKeyInput(new KeyInfo(Key.H, KeyState.Down));

            _controllableDroneMock.Verify(d => d.Control(It.IsAny<DroneCommand>()), Times.Never);
        }

        [Fact]
        public void ProcessKeyInput_KeyNotSupported_ReturnNull()
        {
            MoveCommand moveCommand = null;

            _keyInputEvaluatorMock
                .Setup(kie => kie.EvaluateKey(It.IsAny<KeyInfo>()))
                .Returns(moveCommand);

            var result = _droneController.ProcessKeyInput(new KeyInfo(Key.H, KeyState.Down));

            _controllableDroneMock.Verify(d => d.Control(It.IsAny<DroneCommand>()), Times.Never);

            Assert.Null(result);
        }
    }
}
