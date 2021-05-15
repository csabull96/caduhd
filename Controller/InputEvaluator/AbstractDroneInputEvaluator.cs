namespace Caduhd.Controller.InputEvaluator
{
    /// <summary>
    /// Abstract drone input evaluator.
    /// </summary>
    public abstract class AbstractDroneInputEvaluator
    {
        /// <summary>
        /// The value that represents a lateral movement to the left.
        /// </summary>
        protected const int MOVE_LEFT = NEGATIVE_SIGN_VALUE;

        /// <summary>
        /// The value that represents a lateral movement to the right.
        /// </summary>
        protected const int MOVE_RIGHT = POSITIVE_SIGN_VALUE;

        /// <summary>
        /// The value that represents a longitudinal movement forward.
        /// </summary>
        protected const int MOVE_FORWARD = POSITIVE_SIGN_VALUE;

        /// <summary>
        /// The value that represents a longitudinal movement backward.
        /// </summary>
        protected const int MOVE_BACKWARD = NEGATIVE_SIGN_VALUE;

        /// <summary>
        /// The value that represents a vertical movement upward.
        /// </summary>
        protected const int MOVE_UP = POSITIVE_SIGN_VALUE;

        /// <summary>
        /// The value that represents a vertical movement downward.
        /// </summary>
        protected const int MOVE_DOWN = NEGATIVE_SIGN_VALUE;

        /// <summary>
        /// The value that represents a yaw movement to the left.
        /// </summary>
        protected const int YAW_LEFT = NEGATIVE_SIGN_VALUE;

        /// <summary>
        /// The value that represents a yaw movement to the right.
        /// </summary>
        protected const int YAW_RIGHT = POSITIVE_SIGN_VALUE;

        private const int POSITIVE_SIGN_VALUE = 1;
        private const int NEGATIVE_SIGN_VALUE = -1;
    }
}
