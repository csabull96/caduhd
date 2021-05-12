namespace Caduhd.UserInterface.ViewModel
{
    using Caduhd.Controller.InputAnalyzer;
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Drone.Dji;
    using Caduhd.HandsDetector;
    using Caduhd.Input.Camera;
    using Caduhd.Input.Keyboard;
    using System;
    using System.Windows.Input;


    /// <summary>
    /// Main view model.
    /// </summary>
    public class MainViewModel : BaseViewModel, IDisposable
    {
        private readonly IWebCamera webCamera;
        private readonly KeyEventProcessor keyEventProcessor;

        private CaduhdApp app;

        /// <summary>
        /// Gets the user interface connector for data binding between this view model and the UI.
        /// </summary>
        public UserInterfaceConnector UserInterfaceConnector { get; private set; } = new UserInterfaceConnector();

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

            app = new CaduhdApp(handsAnalyzer, skiColorHandDetector, drone, 
                droneControllerKeyInputEvaluator, droneControllerHandsInputEvaluator);

            app.AttachUI(UserInterfaceConnector);

            this.webCamera = new WebCamera(320, 180);
            this.webCamera.NewFrame += this.ProcessWebCameraFrame;
            this.webCamera.On();
            this.keyEventProcessor = new KeyEventProcessor();
        }

        /// <summary>
        /// Handles the key events coming from the UI.
        /// </summary>
        /// <param name="keyEventArgs">The key event arguments.</param>
        public void HandleKeyEvent(KeyEventArgs keyEventArgs)
        {
            KeyInfo keyInfo = this.keyEventProcessor.ProcessKeyEvent(keyEventArgs.Key, keyEventArgs.IsDown, keyEventArgs.IsRepeat);
            app.Input(keyInfo);
        }

        /// <summary>
        /// Tells the drone controller to connect to the drone.
        /// </summary>
        public void ConnectToDrone()
        {
           
        }

        /// <summary>
        /// Tells the drone controller to turn on the drone's camera.
        /// </summary>
        public void StartStreamingDroneVideo()
        {
            
        }

        /// <summary>
        /// Tells the drone controller to turn off the drone's camera.
        /// </summary>
        public void StopStreamingDroneVideo()
        {
            
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
            app.Input(args.Frame);
        }
    }
}