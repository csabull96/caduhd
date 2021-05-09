using Caduhd.Controller.Command;
using Caduhd.Controller.InputEvaluator;
using Xunit;

namespace Caduhd.Controller.Tests.InputEvaluator
{
    public class DroneControllerKeyInputProcessResultTests
    {
        [Fact]
        public void ResultGetter_ReturnsValueSetThroughConstructor()
        {
            var moveCommand = new MoveCommand(1, 0, 1, 1);
            var inputProcessResult = new DroneControllerKeyInputProcessResult(moveCommand);
            Assert.Equal(moveCommand, inputProcessResult.Result);
        }
    }
}
