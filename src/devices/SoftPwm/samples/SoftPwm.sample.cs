// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Device.Pwm;
using System.Device.Pwm.Drivers;
using System.Diagnostics;
using System.Device.Gpio;
using System.Threading;

class Program
{

    static void Main(string[] args)
    {
        Console.WriteLine("Hello PWM!");

        var PwmController = new PwmController(new SoftPwm());
        PwmController.OpenChannel(17, 0);
        PwmController.StartWriting(17, 0, 200, 0);

        while (true)
        {
            for (int i = 0; i < 100; i++)
            {
                PwmController.ChangeDutyCycle(17, 0, i);
                Thread.Sleep(100);
            }
        }

    }
}
