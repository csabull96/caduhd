namespace Caduhd.Controller.Command
{
    public sealed class StopStreamingVideoCommand : CameraCommand
    {
        public override DroneCommand GetCopy() => new StopStreamingVideoCommand();
    }
}
