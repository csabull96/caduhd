namespace Ksvydo.Input.Camera
{
    public delegate void NewWebCameraFrameEventHandler(object sender, NewWebCameraFrameEventArgs args);

    public interface IWebCamera
    {
        bool IsOn { get; }
        event NewWebCameraFrameEventHandler NewFrame;
        void TurnOn();
        void TurnOff();
    }
}
