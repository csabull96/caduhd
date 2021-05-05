using Caduhd.Controller.Command;
using Caduhd.HandsDetector;
using System;
using System.Drawing;

namespace Caduhd.Controller.InputEvaluator
{
    public class DroneHandsInputEvaluator : AbstractDroneInputEvaluator, IDroneHandsInputEvaluator
    {
        // left and right neutral hand area
        private const double NORMALIZED_NEUTRAL_HAND_AREA_WIDTH = 0.1;
        private const double NORMALIZED_NEUTRAL_HAND_AREA_HEIGHT = 0.35;
        private const double NORMALIZED_LEFT_NEUTRAL_HAND_AREA_X = 0.15;
        private const double NORMALIZED_RIGHT_NEUTRAL_HAND_AREA_X = 1 - NORMALIZED_LEFT_NEUTRAL_HAND_AREA_X - NORMALIZED_NEUTRAL_HAND_AREA_WIDTH;
        private const double NORMALIZED_LEFT_NEUTRAL_HAND_AREA_Y = 0.4;
        private const double NORMALIZED_RIGHT_NEUTRAL_HAND_AREA_Y = NORMALIZED_LEFT_NEUTRAL_HAND_AREA_Y;

        // HANDS INPUT EVALUATOR CONFIGURATION PARAMETERS
        // Lateral
        private const int LATERAL_ANGLE_THRESHOLD = 45;
        // Longitudinal
        private const double LONGITUDINAL_RATIO_THRESHOLD_UPPER = 1.15;
        private const double LONGITUDINAL_RATIO_THRESHOLD_LOWER = 0.6;
        // Vertical
        private const int VERTICAL_ANGLE_THRESHOLD = 130;
        // Yaw
        private const double YAW_RATIO_THRESHOLD_UPPER = 1.4;
        private const double YAW_RATIO_THRESHOLD_LOWER = 0.5;

        // the neutral state of hands (hands as input)
        // when evaluating a custom hands input, we're evaluating the changes relative to this neutral state of hands
        private NormalizedHands _neutralHands;

        public Rectangle GetLeftNeutralHandArea(int imageWidth, int imageHeight) =>
            new Rectangle(
                Convert.ToInt32(NORMALIZED_LEFT_NEUTRAL_HAND_AREA_X * imageWidth),
                Convert.ToInt32(NORMALIZED_LEFT_NEUTRAL_HAND_AREA_Y * imageHeight),
                Convert.ToInt32(NORMALIZED_NEUTRAL_HAND_AREA_WIDTH * imageWidth),
                Convert.ToInt32(NORMALIZED_NEUTRAL_HAND_AREA_HEIGHT * imageHeight));

        public Rectangle GetRightNeutralHandArea(int imageWidth, int imageHeight) =>
            new Rectangle(
                Convert.ToInt32(NORMALIZED_RIGHT_NEUTRAL_HAND_AREA_X * imageWidth),
                Convert.ToInt32(NORMALIZED_RIGHT_NEUTRAL_HAND_AREA_Y * imageHeight),
                Convert.ToInt32(NORMALIZED_NEUTRAL_HAND_AREA_WIDTH * imageWidth),
                Convert.ToInt32(NORMALIZED_NEUTRAL_HAND_AREA_HEIGHT * imageHeight));

        public void Tune(NormalizedHands neutralHands) => _neutralHands = neutralHands;

        public MoveCommand EvaluateHands(NormalizedHands hands)
        {

            //throw new NotImplementedException("Refactor Dude!");


            //dividing by zero problem
            MoveCommand moveCommand = new MoveCommand();

            // this is going to be negative if left hand is lower than center point
            double alpha = 180 / Math.PI * Math.Atan(
                (double)(_neutralHands.Center.Y - hands.Left.Y) / 
                (_neutralHands.Center.X - hands.Left.X));

            double beta = 180 / Math.PI * Math.Atan(
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
                    moveCommand.Vertical = -SIGN_VALUE;
                }
                else if (delta > 0 && isLeftHandUp && isRightHandUp)
                {
                    moveCommand.Vertical = SIGN_VALUE;
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
