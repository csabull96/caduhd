using Caduhd.HandDetector.Detector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Caduhd.UserInterface
{
    public class UserInterfaceConnector : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BitmapSource _currentWebCameraFrame;
        public BitmapSource CurrentWebCameraFrame
        {
            get { return _currentWebCameraFrame; }
            set
            {
                _currentWebCameraFrame = value;
                OnPropertyChanged();
            }
        }

        private BitmapSource _currentDroneCameraFrame;
        public BitmapSource CurrentDroneCameraFrame
        {
            get { return _currentDroneCameraFrame; }
            set
            {
                _currentDroneCameraFrame = value;
                OnPropertyChanged();
            }
        }





        private HandDetectorState m_handDetectorStatus;
        public HandDetectorState HandDetectorStatus
        {
            get { return m_handDetectorStatus; }
            set 
            { 
                m_handDetectorStatus = value;
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

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
