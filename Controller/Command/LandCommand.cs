namespace Caduhd.Controller.Command
{
    public sealed class LandCommand : MovementCommand
    {
        public override DroneCommand Copy() => new LandCommand();

        public override bool Equals(object obj) => obj is LandCommand;
    }
}
