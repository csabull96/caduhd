using Ksvydo.HandDetector;
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

        private ColorBasedHandDetectorState m_handDetectorState;
        public ColorBasedHandDetectorState HandDetectorState
        {
            get { return m_handDetectorState; }
            set 
            { 
                m_handDetectorState = value;
                OnPropertyChanged();
            }
        }

        private string m_leftHand;
        public string LeftHand
        {
            get { return m_leftHand; }
            set 
            { 
                m_leftHand = value;
                OnPropertyChanged();
            }
        }

        private string m_rightHand;
        public string RightHand
        {
            get { return m_rightHand; }
            set
            {
                m_rightHand = value;
                OnPropertyChanged();
            }
        }

        private string m_direction;
        public string Direction
        {
            get { return m_direction; }
            set
            {
                m_direction = value;
                OnPropertyChanged();
            }
        }

        private int m_batteryLevel = 0;
        public void SetBatteryLevel(int batteryLevel)
        {
            m_batteryLevel = batteryLevel;
            OnPropertyChanged(nameof(BatteryLevel));
            OnPropertyChanged(nameof(BatteryPercentage));
        }
        public int BatteryLevel => m_batteryLevel;
        public string BatteryPercentage => $"{m_batteryLevel}%";

        private double m_speed = 0;
        public void SetSpeed(double speed)
        {
            m_speed = speed;
            OnPropertyChanged(nameof(Speed));
        }
        public string Speed { get { return $"Speed: {m_speed.ToString("F1")} m/s"; } }

        private int m_relativeHeight = 0;
        public void SetHeight(int height)
        {
            m_relativeHeight = height;
            OnPropertyChanged(nameof(Height));
        }
        public string Height { get { return $"Height: {m_relativeHeight} cm"; } }

        public UserInterfaceConnector()
        {
            //CurrentWebCameraFrame = new BitmapImage(new Uri(@"Resources\Images\webcam_placeholder.jpg", UriKind.Relative));
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