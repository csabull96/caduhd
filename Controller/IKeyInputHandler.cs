using Caduhd.Controller.InputEvaluator;
using Caduhd.Input.Keyboard;

namespace Caduhd.Controller
{
    public interface IKeyInputHandler
    {
        InputProcessResult ProcessKeyInput(KeyInfo keyInfo);
    }
}