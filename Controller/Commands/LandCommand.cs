namespace Caduhd.Controller.Commands
{
    public sealed class LandCommand : MovementCommand
    {
        public override DroneCommand GetCopy() => new LandCommand();
    }
}
