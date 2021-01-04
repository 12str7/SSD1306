﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Device.Spi;
using System.Threading;
using Iot.Device.MAX31856;
using UnitsNet;

SpiConnectionSettings settings = new(0, 0)
{
    ClockFrequency = MAX31856.SpiClockFrequency,
    Mode = MAX31856.SpiModeSetup,
    DataFlow = MAX31856.SpiDataFlow
};

using SpiDevice device = SpiDevice.Create(settings);
using MAX31856 sensor = new(device, ThermocoupleType.K);
while (true)
{
    var tempColdJunction = sensor.GetColdJunctionTemperature();
    if (sensor.TryGetTemperature(out Temperature temperature))
    {
        Console.WriteLine($"Temperature: {temperature.DegreesFahrenheit:0.0000000} °F, Cold Junction: {tempColdJunction.DegreesFahrenheit:0.00} °F");
    }
    else
    {
        Console.WriteLine($"Error reading temperature, Cold Junction temperature: {tempColdJunction.DegreesFahrenheit:0.00}");
    }

    Thread.Sleep(2000);
}
