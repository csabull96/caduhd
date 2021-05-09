namespace Caduhd.Controller.Command
{
    public sealed class DisconnectCommand : ControlCommand
    {
        public override DroneCommand Copy() => new DisconnectCommand();

        public override bool Equals(object obj) => obj is DisconnectCommand;
    }
}
