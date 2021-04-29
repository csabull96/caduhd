using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Caduhd.UserInterface
{
    public class UserInterfaceConnector : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BitmapSource _currentWebCameraFrame;
        public BitmapSource CurrentWebCameraFrame => _currentWebCameraFrame;
        public void SetCurrentWebCameraFrame(Bitmap bitmap)
        {
            _currentWebCameraFrame = BitmapToBitmapSource(bitmap, PixelFormats.Bgr24);
            OnPropertyChanged(nameof(CurrentWebCameraFrame));
        }

        private BitmapSource _currentDroneCameraFrame;
        public BitmapSource CurrentDroneCameraFrame => _currentDroneCameraFrame;
        public void SetCurrentDroneCameraFrame(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                _currentDroneCameraFrame = BitmapToBitmapSource(bitmap, PixelFormats.Bgr24);
                OnPropertyChanged(nameof(CurrentDroneCameraFrame));
            }
        }

        private string _tuningState;
        public string TuningState
        {
            get { return _tuningState; }
            set 
            { 
                _tuningState = value;
                OnPropertyChanged();
            }
        }

        private string _leftHand;
        public string LeftHand
        {
            get { return _leftHand; }
            set 
            { 
                _leftHand = value;
                OnPropertyChanged();
            }
        }

        private string _rightHand;
        public string RightHand
        {
            get { return _rightHand; }
            set
            {
                _rightHand = value;
                OnPropertyChanged();
            }
        }

        private string _direction;
        public string Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                OnPropertyChanged();
            }
        }

        private int _batteryLevel = 0;
        public void SetBatteryLevel(int batteryLevel)
        {
            _batteryLevel = batteryLevel;
            OnPropertyChanged(nameof(BatteryLevel));
            OnPropertyChanged(nameof(BatteryPercentage));
        }
        public int BatteryLevel => _batteryLevel;
        public string BatteryPercentage => $"{_batteryLevel}%";

        private double _speed = 0;
        public void SetSpeed(double speed)
        {
            _speed = speed;
            OnPropertyChanged(nameof(Speed));
        }
        public string Speed { get { return $"Speed: {_speed.ToString("F1")} m/s"; } }

        private int _relativeHeight = 0;
        public void SetHeight(int height)
        {
            _relativeHeight = height;
            OnPropertyChanged(nameof(Height));
        }
        public string Height { get { return $"Height: {_relativeHeight} cm"; } }

        public UserInterfaceConnector()
        {
            //CurrentWebCameraFrame = new BitmapImage(new Uri(@"Resources\Images\webca_placeholder.jpg", UriKind.Relative));
        }
        private BitmapSource BitmapToBitmapSource(Bitmap bitmap, System.Windows.Media.PixelFormat pixelFormat)
        {
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var bitmapSource = BitmapSource.Create(bitmapData.Width, bitmapData.Height, 96, 96, pixelFormat, null, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}