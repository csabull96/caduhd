namespace Caduhd.UserInterface.ViewModel
{
    using System;
    using System.Windows.Input;
    using Caduhd.Application;
    using Caduhd.Controller.InputAnalyzer;
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Drone.Dji;
    using Caduhd.HandsDetector;
    using Caduhd.Input.Camera;
    using Caduhd.Input.Keyboard;

    /// <summary>
    /// Main view model.
    /// </summary>
    public class MainViewModel : BaseViewModel, IDisposable
    {
        private readonly IWebCamera webCamera;
        private readonly KeyEventProcessor keyEventProcessor;

        private readonly CaduhdApp app;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            var handsAnalyzer = new HandsAnalyzer();
            var skiColorHandDetector = new SkinColorHandsDetector();

            var drone = new Tello();
            var droneControllerKeyInputEvaluator = new TelloKeyInputEvaluator();
            var droneControllerHandsInputEvaluator = new DroneControllerHandsInputEvaluator();

            this.app = new CaduhdApp(handsAnalyzer,
                skiColorHandDetector,
                drone,
                droneControllerKeyInputEvaluator,
                droneControllerHandsInputEvaluator);

            this.app.Bind(this.UserInterfaceConnector);

            this.webCamera = new WebCamera(320, 180);
            this.webCamera.NewFrame += this.ProcessWebCameraFrame;
            this.webCamera.On();
            this.keyEventProcessor = new KeyEventProcessor();
        }

        /// <summary>
        /// Gets the user interface connector for data binding between this view model and the UI.
        /// </summary>
        public UserInterfaceConnector UserInterfaceConnector { get; private set; } = new UserInterfaceConnector();

        /// <summary>
        /// Handles the key events coming from the UI.
        /// </summary>
        /// <param name="keyEventArgs">The key event arguments.</param>
        public void HandleKeyEvent(KeyEventArgs keyEventArgs)
        {
            KeyInfo keyInfo = this.keyEventProcessor.ProcessKeyEvent(keyEventArgs.Key, keyEventArgs.IsDown, keyEventArgs.IsRepeat);
            this.app.Input(keyInfo);
        }

        /// <summary>
        /// Tells the drone controller to connect to the drone.
        /// </summary>
        public void ConnectToDrone()
        {
            this.app.Input(new KeyInfo(Key.D0, KeyState.Down));
            this.app.Input(new KeyInfo(Key.D0, KeyState.Up));
        }

        /// <summary>
        /// Disposes the <see cref="MainViewModel"/>.
        /// </summary>
        public void Dispose()
        {
            (this.webCamera as IDisposable).Dispose();
        }

        private void ProcessWebCameraFrame(object sender, NewWebCameraFrameEventArgs args)
        {
            this.app.Input(args.Frame);
        }
    }
}