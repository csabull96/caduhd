using Caduhd.Controller.Command;

namespace Caduhd.Controller.InputEvaluator
{
    public class DroneControllerHandsInputProcessResult : InputProcessResult
    {
        public MoveCommand Result { get; private set; }

        public DroneControllerHandsInputProcessResult(MoveCommand result)
        {
            Result = result;
        }
    }
}