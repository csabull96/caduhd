namespace Ksvydo.HandDetector
{
    public enum ColorBasedHandDetectorState
    {
        NeedsCalibrating, 
        NeedsReCalibrating,
        ReadyToCaptureBackground,
        ReadyToAnalyzeLeftHand,
        ReadyToAnalyzeRightHand,
        Calibrated,
        Enabled
    }

}
