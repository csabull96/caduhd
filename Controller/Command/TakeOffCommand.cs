namespace Caduhd.Controller.Command
{
    public sealed class TakeOffCommand : MovementCommand
    {
        public override DroneCommand Copy() => new TakeOffCommand();
        public override bool Equals(object obj) => obj is TakeOffCommand;
    }
}
