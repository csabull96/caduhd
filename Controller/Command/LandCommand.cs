namespace Caduhd.Controller.Command
{
    public sealed class LandCommand : MovementCommand
    {
        public override DroneCommand GetCopy() => new LandCommand();
    }
}
