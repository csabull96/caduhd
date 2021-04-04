using Ksvydo.HandDetector;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Caduhd.UserInterface.Converters
{
    public class ColorBasedHandDetectorStateToMessageToUserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string messageToUser = string.Empty;

            try
            {
                ColorBasedHandDetectorState colorBasedHandDetectorState = (ColorBasedHandDetectorState)value;

                switch (colorBasedHandDetectorState)
                {
                    case ColorBasedHandDetectorState.NeedsCalibrating:
                        messageToUser = "The hand detector needs to be calibrated. Please press BS.";
                        break;
                    case ColorBasedHandDetectorState.NeedsReCalibrating:
                        messageToUser = "The hand detector needs to be recalibrated. Please press BS.";
                        break;
                    case ColorBasedHandDetectorState.ReadyToCaptureBackground:
                        messageToUser = "Lower your hands and press BS to capture the background.";
                        break;
                    case ColorBasedHandDetectorState.ReadyToAnalyzeLeftHand:
                        messageToUser = "Raise your left hand only and press BS to analyze it.";
                        break;
                    case ColorBasedHandDetectorState.ReadyToAnalyzeRightHand:
                        messageToUser = "Raise your right hand only and press BS to analyze it.";
                        break;
                    case ColorBasedHandDetectorState.Calibrated:
                        messageToUser = "The hand detector has been calibrated. Press BS to enable it.";
                        break;
                    case ColorBasedHandDetectorState.Enabled:
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
