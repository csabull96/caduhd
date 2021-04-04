using Caduhd.Controller.Commands;
using Ksvydo.HandDetector.Model;

namespace Caduhd.Controller.InputEvaluator
{
    public interface IDroneHandsInputEvaluator
    {
        MoveCommand EvaluateHands(Hands hands);
    }
}
