using Caduhd.Controller.Commands;

namespace Caduhd.Controller
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