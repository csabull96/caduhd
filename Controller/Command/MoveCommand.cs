namespace Caduhd.Controller.Command
{
    public sealed class MoveCommand : MovementCommand
    {
        private const int DEFAULT = 0;
        private const int MIN = -1;
        private const int MAX = 1;

        private int _lateral = DEFAULT;
        public int Lateral
        {
            get { return _lateral; }
            set { _lateral = AdjustValueIfWrong(value); }
        }

        private int _vertical = DEFAULT;
        public int Vertical
        {
            get { return _vertical; }
            set { _vertical = AdjustValueIfWrong(value); }
        }

        private int _longitudinal = DEFAULT;
        public int Longitudinal
        {
            get { return _longitudinal; }
            set { _longitudinal = AdjustValueIfWrong(value); }
        }

        private int _yaw = DEFAULT;
        public int Yaw
        {
            get { return _yaw; }
            set { _yaw = AdjustValueIfWrong(value); }
        }

        public bool Still => _lateral == 0 && _vertical == 0 && _longitudinal == 0 && _yaw == 0;

        public bool Moving => !Still;

        public static MoveCommand Idle => new MoveCommand();

        public override DroneCommand GetCopy()
        {
            DroneCommand moveCommand = new MoveCommand()
            {
                Longitudinal = Longitudinal,
                Lateral = Lateral,
                Vertical = Vertical,
                Yaw = Yaw
            };

            return moveCommand;
        }

        private int AdjustValueIfWrong(int original)
        {
            if (original < MIN)
            {
                return MIN;
            }
            else if (MAX < original)
            {
                return MAX;
            }
            return original;
        }
    }
}
