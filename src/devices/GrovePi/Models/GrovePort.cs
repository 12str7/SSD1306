﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Iot.Device.GrovePiDevice.Models
{
    /// <summary>
    /// The supported Digital Grove Ports
    /// See README.md files for the exact location of each Port
    /// </summary>
    public enum GrovePort
    {
        AnalogPin0 = 0,
        AnalogPin1 = 1,
        AnalogPin2 = 2,
        DigitalPin2 = 2,
        DigitalPin3 = 3,
        DigitalPin4 = 4,
        DigitalPin5 = 5,
        DigitalPin6 = 6,
        DigitalPin7 = 7,
        DigitalPin8 = 8,
        /// <summary>
        /// This is Analog Pin A0 used as Digital Pin
        /// </summary>
        DigitalPin14 = 14,
        /// <summary>
        /// This is Analog Pin A1 used as Digital Pin 
        /// </summary>
        DigitalPin15 = 15,
        /// <summary>
        /// This is Analog Pin A2 used as Digital Pin
        /// </summary>
        DigitalPin16 = 16
    }
}
