namespace Caduhd.Controller.Command
{
    public sealed class StartStreamingVideoCommand : CameraCommand
    {
        public override DroneCommand GetCopy() => new StartStreamingVideoCommand();
    }
}
