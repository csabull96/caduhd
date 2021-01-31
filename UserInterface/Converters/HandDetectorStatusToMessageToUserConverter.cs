using Caduhd.HandDetector.Detector;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Caduhd.UserInterface.Converters
{
    public class HandDetectorStatusToMessageToUserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HandDetectorState handDetectorStatus = HandDetectorState.NeedsCalibrating;
            string messageToUser = string.Empty;

            try
            {
                handDetectorStatus = (HandDetectorState)value;

                switch (handDetectorStatus)
                {
                    case HandDetectorState.NeedsCalibrating:
                        messageToUser = "The hand detector needs to be calibrated. Please press BS.";
                        break;
                    case HandDetectorState.NeedsReCalibrating:
                        messageToUser = "The hand detector needs to be recalibrated. Please press BS.";
                        break;
                    case HandDetectorState.ReadyToCaptureBackground:
                        messageToUser = "Lower your hands and press BS to capture the background.";
                        break;
                    case HandDetectorState.ReadyToAnalyzeLeftHand:
                        messageToUser = "Raise your left hand only and press BS to analyze it.";
                        break;
                    case HandDetectorState.ReadyToAnalyzeRightHand:
                        messageToUser = "Raise your right hand only and press BS to analyze it.";
                        break;
                    case HandDetectorState.Calibrated:
                        messageToUser = "The hand detector has been calibrated. Press BS to enable it.";
                        break;
                    case HandDetectorState.Enabled:
                        messageToUser = "The hand detector is enabled.";
                        break;
                }
            }
            catch (Exception)
            {
                throw new InvalidCastException();
            }

            return messageToUser;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
