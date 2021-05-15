namespace Caduhd.Controller.Tests
{
    using System.Windows.Input;
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Drone;
    using Caduhd.Drone.Command;
    using Caduhd.Input.Keyboard;
    using Moq;
    using Xunit;

    public class KeyboardDroneControllerTests
    {
        private readonly Mock<AbstractDrone> controllableDroneMock;
        private readonly Mock<IDroneControllerKeyInputEvaluator> keyInputEvaluatorMock;
        private readonly KeyboardDroneController droneController;

        public KeyboardDroneControllerTests()
        {
            this.controllableDroneMock = new Mock<AbstractDrone>();
            this.keyInputEvaluatorMock = new Mock<IDroneControllerKeyInputEvaluator>();
            this.droneController = new KeyboardDroneController(this.controllableDroneMock.Object, this.keyInputEvaluatorMock.Object);
        }

        [Fact]
        public void Connect_DroneControlCalledOnce()
        {
            this.droneController.Connect();
            this.controllableDroneMock.Verify(d => d.Control(It.IsAny<ConnectCommand>()), Times.Once);
        }

        [Fact]
        public void StartStreamingVideo_DroneControlCalledOnce()
        {
            this.droneController.StartStreamingVideo();
            this.controllableDroneMock.Verify(d => d.Control(It.IsAny<StartStreamingVideoCommand>()), Times.Once);
        }

        [Fact]
        public void StopStreamingVideo_DroneControlCalledOnce()
        {
            this.droneController.StopStreamingVideo();
            this.controllableDroneMock.Verify(d => d.Control(It.IsAny<StopStreamingVideoCommand>()), Times.Once);
        }

        [Fact]
        public void ProcessKeyInput_KeyIsSupported_DroneControlCalledOnce()
        {
            var moveCommand = new MoveCommand()
            {
                Lateral = 0,
                Longitudinal = 0,
                Vertical = -1,
                Yaw = 0,
            };

            this.keyInputEvaluatorMock
                .Setup(kie => kie.EvaluateKey(It.IsAny<KeyInfo>()))
                .Returns(moveCommand);

            this.droneController.ProcessKeyInput(new KeyInfo(Key.W, KeyState.Down));

            this.controllableDroneMock.Verify(d => d.Control(moveCommand), Times.Once);
        }

        [Fact]
        public void ProcessKeyInput_KeyIsSupported_ReturnsEvaluatedDroneCommand()
        {
            var moveCommand = new MoveCommand()
            {
                Lateral = 0,
                Longitudinal = 0,
                Vertical = -1,
                Yaw = 0,
            };

            this.keyInputEvaluatorMock
                .Setup(kie => kie.EvaluateKey(It.IsAny<KeyInfo>()))
                .Returns(moveCommand);

            var result = this.droneController.ProcessKeyInput(new KeyInfo(Key.W, KeyState.Down));

            this.controllableDroneMock.Verify(d => d.Control(moveCommand), Times.Once);

            Assert.IsType<DroneControllerKeyInputProcessResult>(result);
            DroneCommand resultDroneCommand = (result as DroneControllerKeyInputProcessResult).Result;
            MoveCommand resultMoveCommand = resultDroneCommand as MoveCommand;

            Assert.True(moveCommand.Equals(resultMoveCommand));
        }

        [Fact]
        public void ProcessKeyInput_KeyNotSupported_DroneControlNeverCalled()
        {
            MoveCommand moveCommand = null;

            this.keyInputEvaluatorMock
                .Setup(kie => kie.EvaluateKey(It.IsAny<KeyInfo>()))
                .Returns(moveCommand);

            this.droneController.ProcessKeyInput(new KeyInfo(Key.H, KeyState.Down));

            this.controllableDroneMock.Verify(d => d.Control(It.IsAny<DroneCommand>()), Times.Never);
        }

        [Fact]
        public void ProcessKeyInput_KeyNotSupported_ReturnNull()
        {
            MoveCommand moveCommand = null;

            this.keyInputEvaluatorMock
                .Setup(kie => kie.EvaluateKey(It.IsAny<KeyInfo>()))
                .Returns(moveCommand);

            var result = this.droneController.ProcessKeyInput(new KeyInfo(Key.H, KeyState.Down));

            this.controllableDroneMock.Verify(d => d.Control(It.IsAny<DroneCommand>()), Times.Never);

            Assert.Null(result);
        }
    }
}
