namespace Caduhd.Controller.Command
{
    public sealed class ConnectCommand : ControlCommand
    {
        public override DroneCommand GetCopy() => new ConnectCommand();
    }
}
