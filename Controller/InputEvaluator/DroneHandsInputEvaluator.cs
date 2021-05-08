using Caduhd.Controller.Command;
using Caduhd.HandsDetector;
using System;
using System.Drawing;

namespace Caduhd.Controller.InputEvaluator
{
    public class DroneHandsInputEvaluator : AbstractDroneInputEvaluator, IDroneHandsInputEvaluator
    {
        // left and right neutral hand area
        // should these values be validated??
        private const double NORMALIZED_NEUTRAL_HAND_AREA_WIDTH = 0.1;
        private const double NORMALIZED_NEUTRAL_HAND_AREA_HEIGHT = 0.35;
        private const double NORMALIZED_LEFT_NEUTRAL_HAND_AREA_X = 0.15;
        private const double NORMALIZED_RIGHT_NEUTRAL_HAND_AREA_X = 1 - NORMALIZED_LEFT_NEUTRAL_HAND_AREA_X - NORMALIZED_NEUTRAL_HAND_AREA_WIDTH;
        private const double NORMALIZED_LEFT_NEUTRAL_HAND_AREA_Y = 0.4;
        private const double NORMALIZED_RIGHT_NEUTRAL_HAND_AREA_Y = NORMALIZED_LEFT_NEUTRAL_HAND_AREA_Y;

        // HANDS INPUT EVALUATOR CONFIGURATION PARAMETERS
        // Lateral
        private const int LATERAL_ANGLE_THRESHOLD = 20;
        // Longitudinal
        private const double LONGITUDINAL_UPPER_THRESHOLD = 1.15;
        private const double LONGITUDINAL_LOWER_THRESHOLD = 0.6;
        // Vertical
        private const int VERTICAL_ANGLE_THRESHOLD = 140;
        // Yaw
        private const double YAW_UPPDER_THRESHOLD = 1.4;
        private const double YAW_LOWER_THRESHOLD = 0.5;

        // the neutral state of hands (hands as input)
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
            MoveCommand moveCommand = new MoveCommand();

            Lateral(hands, moveCommand);
            Longitudinal(hands, moveCommand);
            Vertical(hands, moveCommand);
            Yaw(hands, moveCommand);

            return moveCommand;
        }

        private void Lateral(NormalizedHands hands, MoveCommand moveCommand)
        {
            double deltaY = _neutralHands.Right.Y - _neutralHands.Left.Y;
            double deltaX = _neutralHands.Right.X - _neutralHands.Left.X;
            double neutralGradient = deltaY / deltaX;
            double neutralAngle = ConvertRadiansToDegrees(Math.Atan(neutralGradient));

            deltaY = hands.Right.Y - hands.Left.Y;
            deltaX = hands.Right.X - hands.Left.X;
            double actualGradient = deltaY / deltaX;
            double actualAngle = ConvertRadiansToDegrees(Math.Atan(actualGradient));

            double deltaAngle = neutralAngle - actualAngle;
            double absoluteDeltaAngle = Math.Abs(deltaAngle);

            if (LATERAL_ANGLE_THRESHOLD < absoluteDeltaAngle)
            {
                if (deltaAngle < 0)
                    moveCommand.Lateral = MOVE_RIGHT;
                else if (0 < deltaAngle)
                    moveCommand.Lateral = MOVE_LEFT;
            }
        }

        private void Longitudinal(NormalizedHands hands, MoveCommand moveCommand)
        {
            double leftRatio = hands.Left.Weight / _neutralHands.Left.Weight;
            double rightRatio = hands.Right.Weight / _neutralHands.Right.Weight;

            if (LONGITUDINAL_UPPER_THRESHOLD < leftRatio &&
                LONGITUDINAL_UPPER_THRESHOLD < rightRatio)
                moveCommand.Longitudinal = MOVE_FORWARD;
            else if (leftRatio < LONGITUDINAL_LOWER_THRESHOLD &&
                rightRatio < LONGITUDINAL_LOWER_THRESHOLD)
                moveCommand.Longitudinal = MOVE_BACKWARD;
        }

        private void Vertical(NormalizedHands hands, MoveCommand moveCommand)
        {
            double deltaY = _neutralHands.Center.Y - hands.Left.Y;
            double deltaX = _neutralHands.Center.X - hands.Left.X;
            double gradient = deltaY / deltaX;
            double alpha = ConvertRadiansToDegrees(Math.Atan(gradient));
            deltaY = hands.Right.Y - _neutralHands.Center.Y;
            deltaX = hands.Right.X - _neutralHands.Center.X;
            gradient = deltaY / deltaX;
            double beta = ConvertRadiansToDegrees(Math.Atan(gradient));
            double deltaAngle = alpha - beta;

            bool areBothHandsUp = _neutralHands.Left.Y > hands.Left.Y && _neutralHands.Right.Y > hands.Right.Y;
            bool areBothHandsDown = _neutralHands.Left.Y < hands.Left.Y && _neutralHands.Right.Y < hands.Right.Y;

            double angle = 180 - Math.Abs(deltaAngle);
            if (angle < VERTICAL_ANGLE_THRESHOLD)
            {
                if (deltaAngle < 0 && areBothHandsDown)
                    moveCommand.Vertical = MOVE_DOWN;
                else if (deltaAngle > 0 && areBothHandsUp)
                    moveCommand.Vertical = MOVE_UP;
            }
        }

        private void Yaw(NormalizedHands hands, MoveCommand moveCommand)
        {
            double handsWeightRatio = hands.Left.Weight / (_neutralHands.RatioOfLeftWeightToRightWeight * hands.Right.Weight);

            if (handsWeightRatio < YAW_LOWER_THRESHOLD)
                moveCommand.Yaw = YAW_LEFT;
            else if (handsWeightRatio > YAW_UPPDER_THRESHOLD)
                moveCommand.Yaw = YAW_RIGHT;
        }

        private double ConvertRadiansToDegrees(double radians) => 180 / Math.PI * radians;
    }
}
