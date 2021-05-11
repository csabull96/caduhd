namespace Caduhd.Controller.Command
{
    /// <summary>
    /// Move drone command.
    /// </summary>
    public sealed class MoveCommand : MovementCommand
    {
        /// <summary>
        /// The neutral value of a move component.
        /// </summary>
        public const int NEUTRAL = 0;

        /// <summary>
        /// The minimum value of a move component.
        /// </summary>
        public const int MIN = -1;

        /// <summary>
        /// The maximum value of a move component.
        /// </summary>
        public const int MAX = 1;

        private int lateral;
        private int vertical;
        private int longitudinal;
        private int yaw;

        /// <summary>
        /// Gets a newly constructed <see cref="MoveCommand"/> instance that represents a still state.
        /// </summary>
        public static MoveCommand Idle => new MoveCommand();

        /// <summary>
        /// Gets or sets the lateral component of the move.
        /// </summary>
        public int Lateral
        {
            get { return this.lateral; }
            set { this.lateral = this.AdjustValueIfWrong(value); }
        }

        /// <summary>
        /// Gets or sets the vertical component of the move.
        /// </summary>
        public int Vertical
        {
            get { return this.vertical; }
            set { this.vertical = this.AdjustValueIfWrong(value); }
        }

        /// <summary>
        /// Gets or sets the longitudinal component of the move.
        /// </summary>
        public int Longitudinal
        {
            get { return this.longitudinal; }
            set { this.longitudinal = this.AdjustValueIfWrong(value); }
        }

        /// <summary>
        /// Gets or sets the yaw component of the move.
        /// </summary>
        public int Yaw
        {
            get { return this.yaw; }
            set { this.yaw = this.AdjustValueIfWrong(value); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="MoveCommand"/> represents a still state or not.
        /// </summary>
        public bool Still => this.lateral == NEUTRAL && this.vertical == NEUTRAL && this.longitudinal == NEUTRAL && this.yaw == NEUTRAL;

        /// <summary>
        /// Gets a value indicating whether this <see cref="MoveCommand"/> represents a moving state or not.
        /// </summary>
        public bool Moving => !this.Still;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveCommand"/> class.
        /// </summary>
        public MoveCommand()
            : this(NEUTRAL, NEUTRAL, NEUTRAL, NEUTRAL)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveCommand"/> class.
        /// </summary>
        /// <param name="lateral">The lateral component of the move.</param>
        /// <param name="longitudinal">The longitudinal component of the move.</param>
        /// <param name="vertical">The vertical component of the move.</param>
        /// <param name="yaw">The yaw component of the move.</param>
        public MoveCommand(int lateral, int longitudinal, int vertical, int yaw)
        {
            this.Lateral = this.AdjustValueIfWrong(lateral);
            this.Longitudinal = this.AdjustValueIfWrong(longitudinal);
            this.Vertical = this.AdjustValueIfWrong(vertical);
            this.Yaw = this.AdjustValueIfWrong(yaw);
        }

        /// <summary>
        /// Gets a copy of this <see cref="MoveCommand"/>.
        /// </summary>
        /// <returns>The copy of this <see cref="MoveCommand"/> as a <see cref="DroneCommand"/>.</returns>
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

        /// <summary>
        /// The overriden Equals method.
        /// </summary>
        /// <param name="obj">The comparand.</param>
        /// <returns>True if this <see cref="MoveCommand"/> and the provided <paramref name="obj"/> are equal.</returns>
        public override bool Equals(object obj) => obj is MoveCommand other &&
            this.Lateral.Equals(other.Lateral) &&
            this.Longitudinal.Equals(other.Longitudinal) &&
            this.Vertical.Equals(other.Vertical) &&
            this.Yaw.Equals(other.Yaw);
    }
}
