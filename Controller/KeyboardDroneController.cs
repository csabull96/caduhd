using Caduhd.Controller.Commands;
using Caduhd.Controller.InputEvaluator;
using Ksvydo.Input.Keyboard;

namespace Caduhd.Controller
{
    public class KeyboardDroneController : AbstractDroneController, IKeyInputHandler
    {
        protected DroneCommand m_latestEvaluatedKeyInput;
        protected readonly IDroneKeyInputEvaluator m_keyInputEvaluator;

        public KeyboardDroneController(IControllableDrone drone, IDroneKeyInputEvaluator keyInputEvaluator) : base(drone)
        {
            m_keyInputEvaluator = keyInputEvaluator;
        }

        public InputProcessResult ProcessKeyInput(KeyInfo keyInfo)
        {
            DroneCommand keyEvaluated = m_keyInputEvaluator.EvaluateKey(keyInfo);

            if (keyEvaluated == null)
            {
                return null;
            }

            m_latestEvaluatedKeyInput = keyEvaluated;
            DroneControllerKeyInputProcessResult result =
                new DroneControllerKeyInputProcessResult(m_latestEvaluatedKeyInput.GetCopy());
            Control();
            return result;
        }

        public override void Control()
        {
            InternalControl(m_latestEvaluatedKeyInput);
        }
    }
}