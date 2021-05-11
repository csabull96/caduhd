namespace Caduhd.Controller.InputAnalyzer
{
    /// <summary>
    /// Hands analyzer state.
    /// </summary>
    public enum HandsAnalyzerState
    {
        /// <summary>
        /// <see cref="ReadyToAnalyzeLeft"/>
        /// </summary>
        ReadyToAnalyzeLeft,

        /// <summary>
        /// <see cref="AnalyzingLeft"/>
        /// </summary>
        AnalyzingLeft,

        /// <summary>
        /// <see cref="ReadyToAnalyzeRight"/>
        /// </summary>
        ReadyToAnalyzeRight,

        /// <summary>
        /// <see cref="AnalyzingRight"/>
        /// </summary>
        AnalyzingRight,

        /// <summary>
        /// <see cref="Tuning"/>
        /// </summary>
        Tuning,
    }
}
