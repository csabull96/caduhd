namespace Caduhd.Controller.Command
{
    public sealed class ConnectCommand : ControlCommand
    {
        public override DroneCommand Copy() => new ConnectCommand();

        public override bool Equals(object obj) => obj is ConnectCommand;
    }
}
