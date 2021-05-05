using Caduhd.Controller.InputEvaluator;
using Caduhd.HandsDetector;

namespace Caduhd.Controller
{
    public interface IHandsInputHandler
    {
        InputProcessResult ProcessHandsInput(NormalizedHands hands);
    }
}