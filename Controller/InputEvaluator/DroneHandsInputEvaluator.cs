using Caduhd.Controller.Command;
using Caduhd.HandsDetector;
using System;
using System.Drawing;

namespace Caduhd.Controller.InputEvaluator
{
    public class DroneHandsInputEvaluator : AbstractDroneInputEvaluator, IDroneHandsInputEvaluator
    {
        private const int LATERAL_ANGLE_THRESHOLD = 30;

        private const double LONGITUDINAL_RATIO_THRESHOLD_UPPER = 1.3;
        private const double LONGITUDINAL_RATIO_THRESHOLD_LOWER = 0.7;

        private const int VERTICAL_ANGLE_THRESHOLD = 70;

        private const double YAW_RATIO_THRESHOLD_UPPER = 1.5;
        private const double YAW_RATIO_THRESHOLD_LOWER = 0.5;

        private Hands _neutralHands;

        public void Tune(Hands neutralHands) => _neutralHands = neutralHands;

        public Rectangle GetNeutralLeftHandArea(int width, int height)
        {
            int w = width / 10;
            int h = height / 3;
            int x = width / 9;
            int y = height * 2 / 5;

            return new Rectangle(x, y, w, h);
        }
        
        public Rectangle GetNeutralRightHandArea(int width, int height)
        {
            int w = width / 10;
            int h = height / 3;
            int x = width * 8 / 9 - w;
            int y = height * 2 / 5;

            return new Rectangle(x, y, w, h);
        }

        public MoveCommand EvaluateHands(Hands hands)
        {

            throw new NotImplementedException("Refactor Dude!");


            //dividing by zero problem
            MoveCommand moveCommand = new MoveCommand();

            // this is going to be negative if left hand is lower than center point
            double alpha = 180 * Math.Atan(
                (double)(_neutralHands.Center.Y - hands.Left.Y) / 
                (_neutralHands.Center.X - hands.Left.X));

            double beta = 180 * Math.Atan(
                (double)(hands.Right.Y - _neutralHands.Center.Y) /
                (hands.Right.X - _neutralHands.Center.X));

            double delta = alpha - beta;

            // lateral
            double gradient_reference = (double)(_neutralHands.Right.Y - _neutralHands.Left.Y) /
                (_neutralHands.Right.X - _neutralHands.Left.X);
            double angle_reference = 180 * Math.Atan(gradient_reference);

            double gradient_actual = (double)(hands.Right.Y - hands.Left.Y) / 
                (hands.Right.X - hands.Left.X);
            double angle_actual = 180 * Math.Atan(gradient_actual);

            double deltaaa = angle_reference - angle_actual;
            double absoluteDelta = Math.Abs(deltaaa);

            if (deltaaa > 0 && absoluteDelta > LATERAL_ANGLE_THRESHOLD)
            {
                moveCommand.Lateral = -SIGN_VALUE; // move left
            }
            else if (deltaaa < 0 && absoluteDelta > LATERAL_ANGLE_THRESHOLD)
            {
                moveCommand.Lateral = SIGN_VALUE;
            }


            // longitudinal
            // -------o........o-------
            // ------tb--------tt------
            // tb: threshold bottom
            // tt: threshold top

            // make sure result is not int
            double ratioLeft = (double)hands.Left.Weight / _neutralHands.Left.Weight;
            double ratioRight = (double)hands.Right.Weight / _neutralHands.Right.Weight;

            if (ratioLeft > LONGITUDINAL_RATIO_THRESHOLD_UPPER && ratioRight > LONGITUDINAL_RATIO_THRESHOLD_UPPER)
            {
                moveCommand.Longitudinal = SIGN_VALUE; // move forward
            }
            else if (ratioLeft < LONGITUDINAL_RATIO_THRESHOLD_LOWER && ratioRight < LONGITUDINAL_RATIO_THRESHOLD_LOWER)
            {
                moveCommand.Longitudinal = -SIGN_VALUE; // move backward
            }



            // vertical
            // |
            // o  y=1 (actual hand on top means lower y)
            // |
            // |
            // |
            // |
            // |
            // |
            // |
            // o  y=8 (reference hand)
            // |
            bool isLeftHandUp = _neutralHands.Left.Y > hands.Left.Y;
            bool isRightHandUp = _neutralHands.Right.Y > hands.Right.Y;

            bool isLeftHandDown = _neutralHands.Left.Y < hands.Left.Y;
            bool isRightHandDown = _neutralHands.Right.Y < hands.Right.Y;
            // we have alpha and beta and delta

            double angle = 180 - Math.Abs(delta);
            if (angle < VERTICAL_ANGLE_THRESHOLD)
            {
                if (delta < 0 && isLeftHandDown && isRightHandDown)
                {
                    moveCommand.Vertical = SIGN_VALUE;
                }
                else if (delta > 0 && isLeftHandUp && isRightHandUp)
                {
                    moveCommand.Vertical = -SIGN_VALUE;
                }
                else
                {
                    // don't do anything, right?
                }
            }


            // yaw
            // make sure result is not int
            double handsWeightRatio = hands.Left.Weight / (_neutralHands.RatioOfLeftWeightToRightWeight * hands.Right.Weight);

            // -------o........o-------
            // -----ytb........ytt-----
            if (handsWeightRatio < YAW_RATIO_THRESHOLD_LOWER)
            {
                // then yaw left
                moveCommand.Yaw = -SIGN_VALUE;
                
            }
            else if (handsWeightRatio > YAW_RATIO_THRESHOLD_UPPER)
            {
                // then yaw right
                moveCommand.Yaw = SIGN_VALUE;
            }

            return moveCommand;
        }
    }
}
