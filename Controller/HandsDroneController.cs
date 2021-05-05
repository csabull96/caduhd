using Caduhd.Controller.Command;
using Caduhd.Controller.InputEvaluator;
using Caduhd.HandsDetector;

namespace Caduhd.Controller
{
    public class HandsDroneController : KeyboardDroneController, IHandsInputHandler
    {
        private readonly IDroneHandsInputEvaluator _handsInputEvaluator;
        private MoveCommand _latestHandsInputEvaluated;

        public HandsDroneController(IControllableDrone drone, 
            IDroneHandsInputEvaluator handsInputEvaluator, 
            IDroneKeyInputEvaluator keyInputEvaluator) : base(drone, keyInputEvaluator)
        {
            _handsInputEvaluator = handsInputEvaluator;
        }

        public InputProcessResult ProcessHandsInput(NormalizedHands hands)
        {
            _latestHandsInputEvaluated = _handsInputEvaluator.EvaluateHands(hands);
            DroneControllerHandsInputProcessResult result =
                new DroneControllerHandsInputProcessResult(_latestHandsInputEvaluated.GetCopy() as MoveCommand);
            Control();
            return result;
        }

        public override void Control()
        {
            DroneCommand inputsEvaluated = EvaluateInputs();
            InternalControl(inputsEvaluated);
        }

        private DroneCommand EvaluateInputs()
        {
            // key input has always priority over the hands input
            if (_latestKeyInputEvaluated != null)
            {
                DroneCommand latestKeyInputEvaluatedCopy = _latestKeyInputEvaluated.GetCopy();

                // If the evaluated input from the keyboard was executed, then it has to be set to null,
                // otherwise the evaluated hands input will never have the chance to get executed.
                // After being evaluated the _latestKeyInputEvaluated is always set to null
                // unless it's a MoveCommand which represents a moving state.
                // Why?
                // Imagine the following scenario:
                // The hand detector is enabled but we want to control the drone using the keyboard.               
                // We are pressing the left key arrow (nothing else).
                // We are going to enter the KeyboardDroneController.ProcessKeyInput method only once.
                // Right after the key is evaluated the _latestKeyInputEvaluated is set to null.
                // At the same time because of the enabled hand detector we're receiving several input per second from the web camera.
                // This means that even though the left arrow key is still held down, instead of the _latestKeyInputEvaluated
                // the _latestHandsInputEvaluated is going to be executed.
                // (Because the _latestKeyInputEvaluated was set to null before.)
                if (!(_latestKeyInputEvaluated is MoveCommand moveCommand) || moveCommand.Still)
                {
                    _latestKeyInputEvaluated = null;
                }

                return latestKeyInputEvaluatedCopy;
            }
            else if (_latestHandsInputEvaluated != null)
            {
                DroneCommand latestHandsInputEvaluatedCopy = _latestHandsInputEvaluated.GetCopy();
                _latestHandsInputEvaluated = null;
                return latestHandsInputEvaluatedCopy;
            }

            return null;
        }        
    }
}