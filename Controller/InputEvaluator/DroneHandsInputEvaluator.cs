using Caduhd.Controller.Commands;
using Ksvydo.HandDetector.Model;
using System;

namespace Caduhd.Controller.InputEvaluator
{
    public class DroneHandsInputEvaluator : AbstractDroneInputEvaluator, IDroneHandsInputEvaluator
    {
        public MoveCommand EvaluateHands(Hands hands)
        {
            MoveCommand moveCommand = new MoveCommand();

            if (1000 < hands.Left.Weight && 1000 < hands.Right.Weight)
            {
                // longitudinal
                if (4500 < hands.Left.Weight && 4500 < hands.Right.Weight)
                {
                    moveCommand.Longitudinal += SIGN_VALUE;
                }
                else if (hands.Left.Weight < 3000 && hands.Right.Weight < 3000)
                {
                    moveCommand.Longitudinal -= SIGN_VALUE;
                }

                // lateral
                if (Math.Abs(hands.Left.Position.Y - hands.Right.Position.Y) > 50)
                {
                    if (hands.Left.Position.Y > hands.Right.Position.Y)
                    {
                        moveCommand.Lateral -= SIGN_VALUE;
                    }
                    else
                    {
                        moveCommand.Lateral += SIGN_VALUE;
                    }
                }

                // yaw
                if (Math.Abs(hands.Left.Weight - hands.Right.Weight) > 2500)
                {
                    if (hands.Left.Weight > hands.Right.Weight)
                    {
                        moveCommand.Yaw += SIGN_VALUE;
                    }
                    else
                    {
                        moveCommand.Yaw -= SIGN_VALUE;
                    }
                }
            }

            // vertical
            if (hands.Left.Weight > 1000 && hands.Right.Weight > 1000)
            {
                if (hands.Left.Position.Y < 50 && hands.Right.Position.Y < 50)
                {
                    moveCommand.Vertical += SIGN_VALUE;
                }
                else if (140 < hands.Left.Position.Y && 140 < hands.Right.Position.Y)
                {
                    moveCommand.Vertical -= SIGN_VALUE;
                }
            }

            return moveCommand;
        }
    }
}
