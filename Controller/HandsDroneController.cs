using Caduhd.Controller.Commands;
using Caduhd.Controller.InputEvaluator;
using Ksvydo.HandDetector.Model;

namespace Caduhd.Controller
{
    public class HandsDroneController : KeyboardDroneController, IHandsInputHandler
    {
        private MoveCommand m_latestEvaluatedHandsInput;
        private readonly IDroneHandsInputEvaluator m_handsInputEvaluator;

        public HandsDroneController(IControllableDrone drone, 
            IDroneHandsInputEvaluator hie, IDroneKeyInputEvaluator kie) : base(drone, kie)
        {
            m_handsInputEvaluator = hie;
        }

        public InputProcessResult ProcessHandsInput(Hands hands)
        {
            // when there are no hands detected
            // hands should be null

            m_latestEvaluatedHandsInput = m_handsInputEvaluator.EvaluateHands(hands);
            DroneControllerHandsInputProcessResult result =
                new DroneControllerHandsInputProcessResult(m_latestEvaluatedHandsInput.GetCopy() as MoveCommand);
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
            // if the m_latestEvaluatedKeyInput is not null,
            // then it is always prioritized over the m_latestEvaluatedHandsInput
            if (m_latestEvaluatedKeyInput != null)
            {
                DroneCommand copy = m_latestEvaluatedKeyInput.GetCopy();

                // if the evaluated input from the keyboard was executed, then it has to be set to null, otherwise
                // the evaluated hands input will never had the chance to be executed
                // after being evaluated the m_latestEvaluatedKeyInput is always set to null
                // unless it's a MoveCommand which represents a moving state
                // why? if the hand detector is enabled but we want to control the drone using the keyboard
                // imagine: only the left arrow key is held down, we're gonna enter this (ProcessKeyInput) method only once
                // and also set the m_latestEvaluatedKeyInput to null, meanwhile we're receiving several input per second
                // from the web camera, which means that even though the left arrow key is still down
                // (because of m_latestEvaluatedKeyInput was set to null) the evaluated hands input is going to be
                // executed by the drone and not the one from the keyboard
                if (!(m_latestEvaluatedKeyInput is MoveCommand moveCommand) || moveCommand.Still)
                {
                    m_latestEvaluatedKeyInput = null;
                }

                return copy;
            }
            else if (m_latestEvaluatedHandsInput != null)
            {
                DroneCommand copy = m_latestEvaluatedHandsInput.GetCopy();
                m_latestEvaluatedHandsInput = null;
                return copy;
            }

            return null;
        }        
    }
}