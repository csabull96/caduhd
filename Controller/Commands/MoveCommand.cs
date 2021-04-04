namespace Caduhd.Controller.Commands
{
    public sealed class MoveCommand : MovementCommand
    {
        private const int DEFAULT = 0;
        private const int MIN = -1;
        private const int MAX = 1;

        private int m_lateral = DEFAULT;
        public int Lateral
        {
            get { return m_lateral; }
            set { m_lateral = AdjustValueIfWrong(value); }
        }

        private int m_vertical = DEFAULT;
        public int Vertical
        {
            get { return m_vertical; }
            set { m_vertical = AdjustValueIfWrong(value); }
        }

        private int m_longitudinal = DEFAULT;
        public int Longitudinal
        {
            get { return m_longitudinal; }
            set { m_longitudinal = AdjustValueIfWrong(value); }
        }

        private int m_yaw = DEFAULT;
        public int Yaw
        {
            get { return m_yaw; }
            set { m_yaw = AdjustValueIfWrong(value); }
        }

        public bool Still => m_lateral == 0 && m_vertical == 0 && m_longitudinal == 0 && m_yaw == 0;

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
