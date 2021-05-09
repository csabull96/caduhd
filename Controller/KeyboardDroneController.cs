using Caduhd.Controller.Command;
using Caduhd.Controller.InputEvaluator;
using Caduhd.Input.Keyboard;

namespace Caduhd.Controller
{
    public class KeyboardDroneController : AbstractDroneController, IKeyInputHandler
    {
        protected readonly IDroneKeyInputEvaluator _keyInputEvaluator;
        protected DroneCommand _latestKeyInputEvaluated;

        public KeyboardDroneController(IControllableDrone drone, 
            IDroneKeyInputEvaluator keyInputEvaluator) : base(drone)
        {
            _keyInputEvaluator = keyInputEvaluator;
        }

        public InputProcessResult ProcessKeyInput(KeyInfo keyInfo)
        {
            DroneCommand keyEvaluated = _keyInputEvaluator.EvaluateKey(keyInfo);

            if (keyEvaluated == null)
                return null;

            _latestKeyInputEvaluated = keyEvaluated;
            DroneControllerKeyInputProcessResult result =
                new DroneControllerKeyInputProcessResult(_latestKeyInputEvaluated.Copy());
            Control();
            return result;
        }

        protected override void Control()
        {
            InternalControl(_latestKeyInputEvaluated);
        }
    }
}