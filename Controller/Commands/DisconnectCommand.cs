namespace Caduhd.Controller.Commands
{
    public sealed class DisconnectCommand : ControlCommand
    {
        public override DroneCommand GetCopy() => new DisconnectCommand();
    }
}
