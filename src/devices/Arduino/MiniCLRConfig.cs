﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Device.Arduino
{
    [ArduinoReplacement("System.CLRConfig", null, true, IncludingPrivates = true)]
    internal static class MiniCLRConfig
    {
        [ArduinoImplementation(NativeMethod.None)]
        public static bool GetBoolValueWithFallbacks(string switchName, string environmentName, bool defaultValue)
        {
            return defaultValue;
        }
    }
}
