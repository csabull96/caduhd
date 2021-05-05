using Caduhd.Controller.Command;
using Caduhd.HandsDetector;

namespace Caduhd.Controller.InputEvaluator
{
    public interface IDroneHandsInputEvaluator
    {
        MoveCommand EvaluateHands(NormalizedHands hands);
    }
}
