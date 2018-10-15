// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Device.Gpio;
using System.Threading;

namespace pwm_led
{
    class Program
    {
        static void Main(string[] args)
        {
            // GPIO pin 18 maps to chip 0 channel 0 on the Raspberry pi
            int pwmChannel = 0;
            int pwmChip = 0;
            int pwmPeriod = (int)((1f/800f)*1_000_000_000); // Setting the period to 800 MHz
            int pwm80PercentFrequency = (int)(pwmPeriod*0.8f); // Frequency at 80%
            int pwm40PercentFrequency = (int)(pwmPeriod*0.4f); // Frequency at 40%

            using (GpioController controller = new GpioController())
            {
                GpioPWMPin led = controller.OpenPWMPin(pwmChip, pwmChannel);
                led.Period = pwmPeriod;
                for (int i = 0; i < 10; i++)
                {
                    led.DutyCycle = pwm80PercentFrequency;
                    led.PWMWrite(); // Setting frequency to 80%. Should make the LED look brighter.
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    led.DutyCycle = pwm40PercentFrequency;
                    led.PWMWrite(); // Setting frequency to 40%. Should make the LED look less bright.
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
            }
        }
    }
}
