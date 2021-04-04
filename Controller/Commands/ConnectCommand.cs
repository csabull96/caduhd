namespace Caduhd.Controller.Commands
{
    public sealed class ConnectCommand : ControlCommand
    {
        public override DroneCommand GetCopy() => new ConnectCommand();
    }
}
