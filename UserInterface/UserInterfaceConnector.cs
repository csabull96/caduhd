namespace Caduhd.UserInterface
{
    using Caduhd.Common;
    using Caduhd.Drone;
    using Caduhd.Drone.Command;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// User interface connector for data binding between the view model and the UI.
    /// </summary>
    public class UserInterfaceConnector : ICaduhdUIConnector, INotifyPropertyChanged
    {
        private BitmapSource currentWebCameraFrame;
        private BitmapSource currentDroneCameraFrame;
        private string tuningState;
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
        public BitmapSource CurrentWebCameraFrame => this.currentWebCameraFrame;

        /// <summary>
        /// Gets the current drone camera frame.
        /// </summary>
        public BitmapSource CurrentDroneCameraFrame => this.currentDroneCameraFrame;

        /// <summary>
        /// Gets or sets the state of the tuning.
        /// </summary>
        public string TuningState
        {
            get
            {
                return this.tuningState;
            }

            set
            {
                this.tuningState = value;
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
        public string Direction
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
        /// Gets the batter level.
        /// </summary>
        public int BatteryLevel => this.batteryLevel;

        /// <summary>
        /// Gets the battery level as percentage string.
        /// </summary>
        public string BatteryPercentage => $"{this.batteryLevel}%";

        public int WiFiSnr => this.wiFiSnr;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceConnector"/> class.
        /// </summary>
        public UserInterfaceConnector()
        {
        }

        /// <summary>
        /// Sets the current web camera frame.
        /// </summary>
        /// <param name="bitmap">Web camera frame as <see cref="Bitmap"/>.</param>
        public void SetCurrentWebCameraFrame(Bitmap bitmap)
        {
            this.currentWebCameraFrame = this.BitmapToBitmapSource(bitmap, PixelFormats.Bgr24);
            this.OnPropertyChanged(nameof(this.CurrentWebCameraFrame));
        }

        /// <summary>
        /// Sets the current drone camera frame.
        /// </summary>
        /// <param name="bitmap">Drone camera frame as <see cref="Bitmap"/>.</param>
        public void SetCurrentDroneCameraFrame(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                this.currentDroneCameraFrame = this.BitmapToBitmapSource(bitmap, PixelFormats.Bgr24);
                this.OnPropertyChanged(nameof(this.CurrentDroneCameraFrame));
            }
        }





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
        /// <param name="height"></param>
        public void SetHeight(int height)
        {
            this.height = height;
            this.OnPropertyChanged(nameof(this.Height));
        }

        public void SetWiFiSnr(int wiFiSnr)
        {
            this.wiFiSnr = wiFiSnr;
            this.OnPropertyChanged(nameof(this.WiFiSnr));
        }

        /// <summary>
        /// Sets the battery level.
        /// </summary>
        /// <param name="batteryLevel">Battery level</param>
        public void SetBatteryPercentage(int batteryLevel)
        {
            this.batteryLevel = this.batteryLevel;
            this.OnPropertyChanged(nameof(BatteryLevel));
            this.OnPropertyChanged(nameof(BatteryPercentage));
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

        public void SetDroneCameraImage(BgrImage image)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.currentDroneCameraFrame = this.BitmapToBitmapSource(image.Bitmap, PixelFormats.Bgr24);
                this.OnPropertyChanged(nameof(this.CurrentDroneCameraFrame));
            });
        }

        public void SetComputerCameraImage(BgrImage image)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.currentWebCameraFrame = this.BitmapToBitmapSource(image.Bitmap, PixelFormats.Bgr24);
                this.OnPropertyChanged(nameof(this.CurrentWebCameraFrame));
            });
        }

        public void SetEvaluatedHandsInput(MoveCommand handsInputEvaluated)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                 Direction = handsInputEvaluated == null ? "l/r:0 f/b:0 u/d:0 yaw:0" :
                 $"l/r:{handsInputEvaluated.Lateral} f/b:{handsInputEvaluated.Longitudinal} u/d:{handsInputEvaluated.Vertical} yaw:{handsInputEvaluated.Yaw}";
            });
        }

        public void SetDroneState(DroneState droneState)
        {
            SetSpeed(droneState.Speed);
            SetHeight(droneState.Height);
            SetWiFiSnr(droneState.WiFiSnr);
            SetBatteryPercentage(droneState.BatteryPercentage);
        }
    }
}