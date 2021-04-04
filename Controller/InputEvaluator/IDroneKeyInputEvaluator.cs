using Caduhd.Controller.Commands;
using Ksvydo.Input.Keyboard;

namespace Caduhd.Controller.InputEvaluator
{
    public interface IDroneKeyInputEvaluator
    {
        DroneCommand EvaluateKey(KeyInfo keyInfo);
    }
}
