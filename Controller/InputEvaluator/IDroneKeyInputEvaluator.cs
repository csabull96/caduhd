using Caduhd.Controller.Command;
using Caduhd.Input.Keyboard;

namespace Caduhd.Controller.InputEvaluator
{
    public interface IDroneKeyInputEvaluator
    {
        DroneCommand EvaluateKey(KeyInfo keyInfo);
    }
}
