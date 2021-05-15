namespace Caduhd.Controller.InputEvaluator
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using Caduhd.Common;
    using Caduhd.Drone.Command;
    using Caduhd.HandsDetector;

    /// <summary>
    /// Drone hands input evaluator.
    /// </summary>
    public class DroneControllerHandsInputEvaluator : AbstractDroneInputEvaluator, IDroneControllerHandsInputEvaluator, ITuneableDroneControllerHandsInputEvaluator
    {
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
        private NormalizedHands neutralHands;

        /// <summary>
        /// Initializes a new instance of the <see cref="DroneControllerHandsInputEvaluator"/> class.
        /// </summary>
        public DroneControllerHandsInputEvaluator()
        {
            this.TunerHands = new Dictionary<string, Dictionary<string, List<Point>>>();

            this.TunerHands.Add("left", new Dictionary<string, List<Point>>());
            this.TunerHands["left"].Add("poi", new List<Point>());
            this.TunerHands["left"].Add("outline", new List<Point>());

            this.TunerHands.Add("right", new Dictionary<string, List<Point>>());
            this.TunerHands["right"].Add("poi", new List<Point>());
            this.TunerHands["right"].Add("outline", new List<Point>());

            var assembly = Assembly.GetExecutingAssembly();

            var resources = assembly
                .GetManifestResourceNames()
                .Where(r => r.EndsWith("_neutral_hand.png"));

            string resource = string.Empty;
            BgrImage left;
            BgrImage right;

            resource = resources.Single(r => r.Contains("left_neutral_hand.png"));
            using (var stream = assembly.GetManifestResourceStream(resource))
            {
                left = new BgrImage(new Bitmap(stream));
            }

            resource = resources.Single(r => r.Contains("right_neutral_hand.png"));
            using (var stream = assembly.GetManifestResourceStream(resource))
            {
                right = new BgrImage(new Bitmap(stream));
            }

            for (int y = 0; y < left.Height; y++)
            {
                for (int x = 0; x < right.Width; x++)
                {
                    var leftPixel = left.GetPixel(x, y);
                    var rightPixel = right.GetPixel(x, y);

                    if (!leftPixel.Equals(Color.White))
                    {
                        if (leftPixel.Equals(Color.Black))
                        {
                            this.TunerHands["left"]["outline"].Add(new Point(x, y));
                        }
                        else
                        {
                            this.TunerHands["left"]["poi"].Add(new Point(x, y));
                        }
                    }

                    if (!rightPixel.Equals(Color.White))
                    {
                        if (rightPixel.Equals(Color.Black))
                        {
                            this.TunerHands["right"]["outline"].Add(new Point(x, y));
                        }
                        else if (rightPixel.Equals(Color.Blue))
                        {
                            this.TunerHands["right"]["poi"].Add(new Point(x, y));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets dictionary that contains the points of the predefined tuner hands.
        /// </summary>
        public Dictionary<string, Dictionary<string, List<Point>>> TunerHands { get; private set; }

        /// <summary>
        /// Tuner method to set the neutral hands as a reference for future hands input evaluation.
        /// </summary>
        /// <param name="neutralHands">Hands detected in their neutral position.</param>
        public void Tune(NormalizedHands neutralHands) => this.neutralHands = neutralHands;

        /// <summary>
        /// Evaluates hands to <see cref="MoveCommand"/>.
        /// </summary>
        /// <param name="hands">Hands to evaluate.</param>
        /// <returns>The evaluated hands as a <see cref="MoveCommand"/>.</returns>
        public MoveCommand EvaluateHands(NormalizedHands hands)
        {
            MoveCommand moveCommand = new MoveCommand();

            this.Lateral(hands, moveCommand);
            this.Longitudinal(hands, moveCommand);
            this.Vertical(hands, moveCommand);
            this.Yaw(hands, moveCommand);

            return moveCommand;
        }

        private void Lateral(NormalizedHands hands, MoveCommand moveCommand)
        {
            double deltaY = this.neutralHands.Right.Y - this.neutralHands.Left.Y;
            double deltaX = this.neutralHands.Right.X - this.neutralHands.Left.X;
            double neutralGradient = deltaY / deltaX;
            double neutralAngle = this.ConvertRadiansToDegrees(Math.Atan(neutralGradient));

            deltaY = hands.Right.Y - hands.Left.Y;
            deltaX = hands.Right.X - hands.Left.X;
            double actualGradient = deltaY / deltaX;
            double actualAngle = this.ConvertRadiansToDegrees(Math.Atan(actualGradient));

            double deltaAngle = neutralAngle - actualAngle;
            double absoluteDeltaAngle = Math.Abs(deltaAngle);

            if (LATERAL_ANGLE_THRESHOLD < absoluteDeltaAngle)
            {
                if (deltaAngle < 0)
                {
                    moveCommand.Lateral = MOVE_RIGHT;
                }
                else if (0 < deltaAngle)
                {
                    moveCommand.Lateral = MOVE_LEFT;
                }
            }
        }

        private void Longitudinal(NormalizedHands hands, MoveCommand moveCommand)
        {
            double leftRatio = hands.Left.Weight / this.neutralHands.Left.Weight;
            double rightRatio = hands.Right.Weight / this.neutralHands.Right.Weight;

            if (LONGITUDINAL_UPPER_THRESHOLD < leftRatio && LONGITUDINAL_UPPER_THRESHOLD < rightRatio)
            {
                moveCommand.Longitudinal = MOVE_FORWARD;
            }
            else if (leftRatio < LONGITUDINAL_LOWER_THRESHOLD && rightRatio < LONGITUDINAL_LOWER_THRESHOLD)
            {
                moveCommand.Longitudinal = MOVE_BACKWARD;
            }
        }

        private void Vertical(NormalizedHands hands, MoveCommand moveCommand)
        {
            double deltaY = this.neutralHands.Center.Y - hands.Left.Y;
            double deltaX = this.neutralHands.Center.X - hands.Left.X;
            double gradient = deltaY / deltaX;
            double alpha = this.ConvertRadiansToDegrees(Math.Atan(gradient));
            deltaY = hands.Right.Y - this.neutralHands.Center.Y;
            deltaX = hands.Right.X - this.neutralHands.Center.X;
            gradient = deltaY / deltaX;
            double beta = this.ConvertRadiansToDegrees(Math.Atan(gradient));
            double deltaAngle = alpha - beta;

            bool areBothHandsUp = this.neutralHands.Left.Y > hands.Left.Y && this.neutralHands.Right.Y > hands.Right.Y;
            bool areBothHandsDown = this.neutralHands.Left.Y < hands.Left.Y && this.neutralHands.Right.Y < hands.Right.Y;

            double angle = 180 - Math.Abs(deltaAngle);
            if (angle < VERTICAL_ANGLE_THRESHOLD)
            {
                if (deltaAngle < 0 && areBothHandsDown)
                {
                    moveCommand.Vertical = MOVE_DOWN;
                }
                else if (deltaAngle > 0 && areBothHandsUp)
                {
                    moveCommand.Vertical = MOVE_UP;
                }
            }
        }

        private void Yaw(NormalizedHands hands, MoveCommand moveCommand)
        {
            double handsWeightRatio = hands.Left.Weight / (this.neutralHands.RatioOfLeftWeightToRightWeight * hands.Right.Weight);

            if (handsWeightRatio < YAW_LOWER_THRESHOLD)
            {
                moveCommand.Yaw = YAW_LEFT;
            }
            else if (handsWeightRatio > YAW_UPPDER_THRESHOLD)
            {
                moveCommand.Yaw = YAW_RIGHT;
            }
        }

        private double ConvertRadiansToDegrees(double radians) => 180 / Math.PI * radians;
    }
}
