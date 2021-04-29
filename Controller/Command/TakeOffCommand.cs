namespace Caduhd.Controller.Command
{
    public sealed class TakeOffCommand : MovementCommand
    {
        public override DroneCommand GetCopy() => new TakeOffCommand();
    }
}
