namespace Caduhd.UserInterface
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Caduhd.Application;
    using Caduhd.Common;
    using Caduhd.Controller.InputAnalyzer;
    using Caduhd.Drone;
    using Caduhd.Drone.Command;

    /// <summary>
    /// User interface connector for data binding between the view model and the UI.
    /// </summary>
    public class UserInterfaceConnector : ICaduhdUIConnector, INotifyPropertyChanged
    {
        private BitmapSource webCameraFrame;
        private BitmapSource droneCameraFrame;
        private HandsAnalyzerState handsAnalyzerState;
        private string leftHand;
        private string rightHand;
        private string direction;
        private double speed = 0;
        private int height = 0;
        private int wiFiSnr = 0;
        private int batteryLevel = 0;

        /// <summary>
        /// Property changed event to notify UI about changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the current web camera frame.
        /// </summary>
        public BitmapSource CurrentWebCameraFrame => this.webCameraFrame;

        /// <summary>
        /// Gets the current drone camera frame.
        /// </summary>
        public BitmapSource CurrentDroneCameraFrame => this.droneCameraFrame;

        /// <summary>
        /// Gets or sets the state of the tuning.
        /// </summary>
        public HandsAnalyzerState HandsAnalyzerState
        {
            get
            {
                return this.handsAnalyzerState;
            }

            set
            {
                this.handsAnalyzerState = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the representation of the left hand.
        /// </summary>
        public string LeftHand
        {
            get
            {
                return this.leftHand;
            }

            set
            {
                this.leftHand = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the representation of the right hand.
        /// </summary>
        public string RightHand
        {
            get
            {
                return this.rightHand;
            }

            set
            {
                this.rightHand = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the direction representation.
        /// </summary>
        public string EvaluatedHandsInput
        {
            get
            {
                return this.direction;
            }

            set
            {
                this.direction = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the speed as string with metric unit.
        /// </summary>
        public string Speed => $"Speed: {this.speed:F1} m/s";

        /// <summary>
        /// Gets the height string with metric unit.
        /// </summary>
        public string Height => $"Height: {this.height} cm";

        /// <summary>
        /// Gets the Wi-Fi's signal-to-noise ratio.
        /// </summary>
        public int WiFiSnr => this.wiFiSnr;

        /// <summary>
        /// Gets the batter level.
        /// </summary>
        public int BatteryLevel => this.batteryLevel;

        /// <summary>
        /// Gets the battery level as percentage string.
        /// </summary>
        public string BatteryPercentage => $"{this.batteryLevel}%";

        /// <summary>
        /// Sets the speed.
        /// </summary>
        /// <param name="speed">The speed.</param>
        public void SetSpeed(double speed)
        {
            this.speed = speed;
            this.OnPropertyChanged(nameof(this.Speed));
        }

        /// <summary>
        /// Sets the height.
        /// </summary>
        /// <param name="height">The height of the flight.</param>
        public void SetHeight(int height)
        {
            this.height = height;
            this.OnPropertyChanged(nameof(this.Height));
        }

        /// <summary>
        /// Sets the Wi-Fi's signal-to-noise ratio.
        /// </summary>
        /// <param name="wiFiSnr">Wi-Fi signal-to-noise ratio.</param>
        public void SetWiFiSnr(int wiFiSnr)
        {
            this.wiFiSnr = wiFiSnr;
            this.OnPropertyChanged(nameof(this.WiFiSnr));
        }

        /// <summary>
        /// Sets the battery level.
        /// </summary>
        /// <param name="batteryLevel">Battery level.</param>
        public void SetBatteryLevel(int batteryLevel)
        {
            this.batteryLevel = batteryLevel;
            this.OnPropertyChanged(nameof(this.BatteryLevel));
            this.OnPropertyChanged(nameof(this.BatteryPercentage));
        }

        /// <summary>
        /// Sets the image coming from the drone's camera.
        /// </summary>
        /// <param name="image">The image received from the drone's camera.</param>
        public void SetDroneCameraImage(BgrImage image)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                this.droneCameraFrame = this.BitmapToBitmapSource(image.Bitmap, PixelFormats.Bgr24);
                this.OnPropertyChanged(nameof(this.CurrentDroneCameraFrame));
            });
        }

        /// <summary>
        /// Sets the image coming from the computer's primary camera.
        /// </summary>
        /// <param name="image">The image from the computer's primary camera.</param>
        public void SetComputerCameraImage(BgrImage image)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                this.webCameraFrame = this.BitmapToBitmapSource(image.Bitmap, PixelFormats.Bgr24);
                this.OnPropertyChanged(nameof(this.CurrentWebCameraFrame));
            });
        }

        /// <summary>
        /// Sets the state of the drone.
        /// </summary>
        /// <param name="droneState">The state of the drone.</param>
        public void SetDroneState(DroneState droneState)
        {
            this.SetSpeed(droneState.Speed);
            this.SetHeight(droneState.Height > 0 ? droneState.Height : droneState.ToF);
            this.SetWiFiSnr(droneState.WiFiSnr);
            this.SetBatteryLevel(droneState.BatteryPercentage);
        }

        /// <summary>
        /// Sets the state of the hands analyzer tuning.
        /// </summary>
        /// <param name="handsAnalyzerState">The state of the hands analyzer.</param>
        public void SetHandsAnalyzerState(HandsAnalyzerState handsAnalyzerState)
        {
            this.HandsAnalyzerState = handsAnalyzerState;
        }

        /// <summary>
        /// Sets the evaluated hands input.
        /// </summary>
        /// <param name="handsInputEvaluated">The evaluated hands input as <see cref="MoveCommand"/>.</param>
        public void SetEvaluatedHandsInput(MoveCommand handsInputEvaluated)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                 this.EvaluatedHandsInput = handsInputEvaluated == null ? "l/r:0 f/b:0 u/d:0 yaw:0" :
                 $"l/r:{handsInputEvaluated.Lateral} f/b:{handsInputEvaluated.Longitudinal} u/d:{handsInputEvaluated.Vertical} yaw:{handsInputEvaluated.Yaw}";
            });
        }

        /// <summary>
        /// Fires the <see cref="PropertyChanged"/> event to notify the UI about a change.
        /// </summary>
        /// <param name="name">The name of the property changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private BitmapSource BitmapToBitmapSource(Bitmap bitmap, System.Windows.Media.PixelFormat pixelFormat)
        {
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var bitmapSource = BitmapSource.Create(bitmapData.Width, bitmapData.Height, 96, 96, pixelFormat, null, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }
    }
}