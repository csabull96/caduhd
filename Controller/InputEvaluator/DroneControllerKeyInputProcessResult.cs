using Caduhd.Controller.Command;

namespace Caduhd.Controller.InputEvaluator
{
    public class DroneControllerKeyInputProcessResult : InputProcessResult
    {
        public DroneCommand Result { get; private set; }

        public DroneControllerKeyInputProcessResult(DroneCommand result)
        {
            Result = result;
        }
    }
}