namespace Caduhd.Controller.Command
{
    public sealed class StopStreamingVideoCommand : CameraCommand
    {
        public override DroneCommand Copy() => new StopStreamingVideoCommand();
        public override bool Equals(object obj) => obj is StopStreamingVideoCommand;
    }
}
