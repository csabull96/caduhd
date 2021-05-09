namespace Caduhd.Controller.Command
{
    public sealed class MoveCommand : MovementCommand
    {
        public const int NEUTRAL = 0;
        public const int MIN = -1;
        public const int MAX = 1;

        private int _lateral;
        public int Lateral
        {
            get { return _lateral; }
            set { _lateral = AdjustValueIfWrong(value); }
        }

        private int _vertical;
        public int Vertical
        {
            get { return _vertical; }
            set { _vertical = AdjustValueIfWrong(value); }
        }

        private int _longitudinal;
        public int Longitudinal
        {
            get { return _longitudinal; }
            set { _longitudinal = AdjustValueIfWrong(value); }
        }

        private int _yaw;
        public int Yaw
        {
            get { return _yaw; }
            set { _yaw = AdjustValueIfWrong(value); }
        }

        public bool Still => _lateral == NEUTRAL && _vertical == NEUTRAL && _longitudinal == NEUTRAL && _yaw == NEUTRAL;

        public bool Moving => !Still;

        public static MoveCommand Idle => new MoveCommand();

        public MoveCommand() : this(NEUTRAL, NEUTRAL, NEUTRAL, NEUTRAL) { }

        public MoveCommand(int lateral, int longitudinal, int vertical, int yaw)
        {
            Lateral = AdjustValueIfWrong(lateral);
            Longitudinal = AdjustValueIfWrong(longitudinal);
            Vertical = AdjustValueIfWrong(vertical);
            Yaw = AdjustValueIfWrong(yaw);
        }

        public override DroneCommand Copy()
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

        public override bool Equals(object obj) => obj is MoveCommand other &&
            this.Lateral.Equals(other.Lateral) &&
            this.Longitudinal.Equals(other.Longitudinal) &&
            this.Vertical.Equals(other.Vertical) &&
            this.Yaw.Equals(other.Yaw);
    }
}
