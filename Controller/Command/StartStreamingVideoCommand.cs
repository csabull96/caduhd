namespace Caduhd.Controller.Command
{
    public sealed class StartStreamingVideoCommand : CameraCommand
    {
        public override DroneCommand Copy() => new StartStreamingVideoCommand();

        public override bool Equals(object obj) => obj is StartStreamingVideoCommand;
    }
}
