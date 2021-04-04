namespace Caduhd.Controller.Commands
{
    public sealed class TakeOffCommand : MovementCommand
    {
        public override DroneCommand GetCopy() => new TakeOffCommand();
    }
}
