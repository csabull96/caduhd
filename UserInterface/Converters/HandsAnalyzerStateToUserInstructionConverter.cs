namespace Caduhd.UserInterface.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using Caduhd.Controller.InputAnalyzer;

    /// <summary>
    /// Hands analyzer state to user instruction converter.
    /// </summary>
    public class HandsAnalyzerStateToUserInstructionConverter : IValueConverter
    {
        /// <summary>
        /// Converts the hands analyzer state to user instruction.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The user instruction based on the input value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (value is HandsAnalyzerState handsAnalyzerState)
            {
                switch (handsAnalyzerState)
                {
                    case HandsAnalyzerState.ReadyToAnalyzeLeft:
                    case HandsAnalyzerState.AnalyzingLeft:
                        result = "Lower your right hand and press BS to analyze your left one.";
                        break;
                    case HandsAnalyzerState.ReadyToAnalyzeRight:
                    case HandsAnalyzerState.AnalyzingRight:
                        result = "Lower your right hand and press BS to analyze your left one.";
                        break;
                    case HandsAnalyzerState.Tuning:
                        result = "The hands detector is tuned.";
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the user instruction back to hands analyzer state.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The result of the reverse conversion.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
