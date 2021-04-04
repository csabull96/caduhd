using Caduhd.Controller.Commands;

namespace Caduhd.Controller
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