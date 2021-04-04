using Ksvydo.HandDetector.Model;

namespace Caduhd.Controller
{
    public interface IHandsInputHandler
    {
        InputProcessResult ProcessHandsInput(Hands hands);
    }
}