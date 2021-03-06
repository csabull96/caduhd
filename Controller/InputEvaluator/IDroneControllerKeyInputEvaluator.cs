﻿namespace Caduhd.Controller.InputEvaluator
{
    using Caduhd.Drone.Command;
    using Caduhd.Input.Keyboard;

    /// <summary>
    /// <see cref="IDroneControllerKeyInputEvaluator"/> interface.
    /// </summary>
    public interface IDroneControllerKeyInputEvaluator
    {
        /// <summary>
        /// Evaluates <see cref="KeyInfo"/> input.
        /// </summary>
        /// <param name="keyInfo"><see cref="KeyInfo"/> to evaluate.</param>
        /// <returns>The evaluated <paramref name="keyInfo"/> as <see cref="DroneCommand"/>.</returns>
        DroneCommand EvaluateKey(KeyInfo keyInfo);
    }
}
