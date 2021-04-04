namespace Caduhd.Controller.Commands
{
    public sealed class StopStreamingVideoCommand : CameraCommand
    {
        public override DroneCommand GetCopy() => new StopStreamingVideoCommand();
    }
}
