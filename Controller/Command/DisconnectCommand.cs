namespace Caduhd.Controller.Command
{
    public sealed class DisconnectCommand : ControlCommand
    {
        public override DroneCommand GetCopy() => new DisconnectCommand();
    }
}
