namespace Caduhd.Controller.InputEvaluator
{
    public abstract class AbstractDroneInputEvaluator
    {
        private const int POSITIVE_SIGN_VALUE = 1;
        private const int NEGATIVE_SIGN_VALUE = -1;

        protected const int MOVE_LEFT = NEGATIVE_SIGN_VALUE;
        protected const int MOVE_RIGHT = POSITIVE_SIGN_VALUE;
        protected const int MOVE_UP = POSITIVE_SIGN_VALUE;
        protected const int MOVE_DOWN = NEGATIVE_SIGN_VALUE;
        protected const int MOVE_FORWARD = POSITIVE_SIGN_VALUE;
        protected const int MOVE_BACKWARD = NEGATIVE_SIGN_VALUE;
        protected const int YAW_LEFT = NEGATIVE_SIGN_VALUE;
        protected const int YAW_RIGHT = POSITIVE_SIGN_VALUE;
    }
}
