namespace Caduhd.Controller.Tests.InputEvaluator
{
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Drone.Command;
    using Xunit;

    public class DroneControllerHandsInputProcessResultTests
    {
        [Fact]
        public void ResultGetter_ReturnsValueSetThroughConstructor()
        {
            var moveCommand = new MoveCommand(1, 0, 1, 1);
            var inputProcessResult = new DroneControllerHandsInputProcessResult(moveCommand);
            Assert.Equal(moveCommand, inputProcessResult.Result);
        }
    }
}
