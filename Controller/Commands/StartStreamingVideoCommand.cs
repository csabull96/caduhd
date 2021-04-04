namespace Caduhd.Controller.Commands
{
    public sealed class StartStreamingVideoCommand : CameraCommand
    {
        public override DroneCommand GetCopy() => new StartStreamingVideoCommand();
    }
}
