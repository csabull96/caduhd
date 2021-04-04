﻿using Caduhd.Controller.Commands;
using Ksvydo.Input.Keyboard;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace Caduhd.Controller.InputEvaluator
{
    public class TelloKeyInputEvaluator : GeneralDroneKeyInputEvaluator
    {
        private const Key START_STREAMING_VIDEO = Key.RightShift;
        private const Key STOP_STREAMING_VIDEO = Key.LeftShift;

        protected KeyInfo StartStreamingVideo { get; private set; } = new KeyInfo(START_STREAMING_VIDEO);
        protected KeyInfo StopStreamingVideo { get; private set; } = new KeyInfo(STOP_STREAMING_VIDEO);

        protected override DroneCommand EvaluateInputKeys()
        {
            if (StartStreamingVideo.KeyState == KeyState.Down)
            {
                return new StartStreamingVideoCommand();
            }
            else if (StopStreamingVideo.KeyState == KeyState.Down)
            {
                return new StopStreamingVideoCommand();
            }
            else
            {
                return base.EvaluateInputKeys();
            }
        }

        protected override IEnumerable<KeyInfo> GetInputKeys()
        {
            return typeof(TelloKeyInputEvaluator)
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(pi => pi.PropertyType == typeof(KeyInfo))
                .Select(pi => (KeyInfo)pi.GetValue(this));
        }
    }
}
