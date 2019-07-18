﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Device.Pwm.Drivers;
using System.IO;

namespace Iot.Device.Servo
{
    public class ServoMotor : IDisposable
    {
        private double _pulseFrequency = 20.0;
        private double _angle;
        double _currentPulseWidth = 0;
        
        /// <summary>
        /// Are we running on hardware or software PWM? True for hardware, false for sofware
        /// </summary>
        public bool IsRunningHardwarePwm { get; private set; }

        /// <summary>
        /// Servo motor definition.
        /// </summary>
        private ServoMotorDefinition _definition;

        /// <summary>
        /// The duration per angle's degree.
        /// </summary>
        private double _rangePerDegree;
        private PwmChannel _pwmChannel;

        /// <summary>
        /// Initialize a <see cref="ServoMotor"/> using a specified PwmChannel
        /// </summary>
        /// <param name="pwmChannel">The <see cref="PwmChannel"/> to be used to control this servo.</param>
        /// <param name="definition">The <see cref="ServoMotorDefinition"/> to be used</param>
        public ServoMotor(PwmChannel pwmChannel, ServoMotorDefinition definition)
        {
            _definition = definition;
            _pulseFrequency = definition.PeriodMicroseconds / 1000.0;

            UpdateRange();
            _pwmChannel = pwmChannel;

            _pwmChannel.DutyCyclePercentage = (1 - (_pulseFrequency - _currentPulseWidth) / _pulseFrequency) * 100;
            _pwmChannel.Start();

        }

        /// <summary>
        /// Initialize a <see cref="ServoMotor"/> using <see cref="SoftwarePwmChannel"/> on <paramref name="pinNumber"/>
        /// </summary>
        /// <param name="pinNumber">The pin that will be used for the software pwm</param>
        /// <param name="definition">The <see cref="ServoMotorDefinition"/> to be used</param>
        public ServoMotor(int pinNumber, ServoMotorDefinition definition)
            : this(CreatePwmChannel(pinNumber, 0, 0, definition, false), definition)
        {
            IsRunningHardwarePwm = false;
        }

        /// <summary>
        /// Initialize a <see cref="ServoMotor"/> using Hardware Pwm on <paramref name="chip"/> and <paramref name="channel"/>
        /// </summary>
        /// <param name="chip">The chip number for Hardware Pwm</param>
        /// <param name="channel">The channel number for Hardware Pwm</param>
        /// <param name="definition">The <see cref="ServoMotorDefinition"/> to be used</param>
        public ServoMotor(int chip, int channel, ServoMotorDefinition definition)
            : this(CreatePwmChannel(-1, chip, channel, definition, true), definition)
        {
            IsRunningHardwarePwm = true;
        }

        private static PwmChannel CreatePwmChannel(int pinNumber, int chip, int channel, ServoMotorDefinition definition, bool useHardwarePwm)
        {
            if (useHardwarePwm)
            {
                return PwmChannel.Create(chip, channel, (int)(1000000 / definition.PeriodMicroseconds));
            }
            else
            {
                return new SoftwarePwmChannel(pinNumber, (int)(1000000 / definition.PeriodMicroseconds));
            }
        }

        /// <summary>
        /// Rotate the servomotor with a specific pulse in microseconds
        /// </summary>
        /// <param name="durationMicroSec">Pulse duration in microseconds</param>
        public void SetPulse(uint durationMicroSec)
        {
            _currentPulseWidth = durationMicroSec / 1000.0;
            _pwmChannel.DutyCyclePercentage = (1 - (_pulseFrequency - _currentPulseWidth) / _pulseFrequency) * 100;
        }

        /// <summary>
        /// Rotate the servomotor with a specific pulse in microseconds
        /// </summary>
        /// <param name="periodMicroSec">Period duration in microseconds</param>
        /// <param name="durationMicroSec">Pulse duration in microseconds</param>
        /// <remarks>Changing the period will override the period definition for the motor. It is not recommended to do so.</remarks>
        public void SetPulse(uint periodMicroSec, uint durationMicroSec)
        {
            _definition.PeriodMicroseconds = periodMicroSec;
            _pulseFrequency = periodMicroSec / 1000.0;
            _currentPulseWidth = durationMicroSec / 1000.0;
            _pwmChannel.DutyCyclePercentage = (1 - (_pulseFrequency - _currentPulseWidth) / _pulseFrequency) * 100;
        }

        /// <summary>
        /// Is it moving? Return true if the servomotor is currently moving.
        /// </summary>
        public bool IsMoving => _currentPulseWidth != 0;

        /// <summary>
        /// Get the current angle or set to move the motor.
        /// The minimum angle is always 0, maximum is user defined and by default is 360°.
        /// The maximum angle is passed in the constructor thru the ServoMotorDefinition.
        /// When passing 100 as the maximum angle, you will use angle as a percentage 
        /// from 0 = minimum rotation of the servomotor to 100 = maximum rotation
        /// </summary>
        /// <remarks>Motor won't move until Angle is set explicitly.</remarks>
        public double Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                double newAngle = Math.Clamp(value, 0, _definition.MaximumAngle);
                if (newAngle != Angle)
                {
                    _angle = newAngle;
                    Rotate();
                }
            }
        }

        /// <summary>
        /// Rotate the motor to current <see cref="Angle"/>.
        /// </summary>
        private void Rotate()
        {
            double duration = (_definition.MinimumDurationMicroseconds + _rangePerDegree * Angle) / 1000.0;
            _currentPulseWidth = duration;
            _pwmChannel.DutyCyclePercentage = (1 - (_pulseFrequency - _currentPulseWidth) / _pulseFrequency) * 100;
        }

        /// <summary>
        /// Updates the temporary variables when definition changes.
        /// </summary>
        private void UpdateRange()
        {
            if (_definition.MaximumAngle != 0)
                _rangePerDegree = (_definition.MaximumDurationMicroseconds - _definition.MinimumDurationMicroseconds) / _definition.MaximumAngle;
        }

        public void Dispose()
        {
            _pwmChannel?.Dispose();
            _pwmChannel = null;
        }

        /// <summary>
        /// Minimum duration for shortest pulse expressed in microseconds that servo supports.
        /// </summary>
        public uint MinimumDurationMicroseconds
        {
            get { return _definition.MinimumDurationMicroseconds; }
            set
            {
                if (_definition.MinimumDurationMicroseconds != value)
                {
                    _definition.MinimumDurationMicroseconds = value;
                    UpdateRange();
                }
            }
        }

        /// <summary>
        /// Maximum duration for longest pulse expressed in microseconds that servo supports.
        /// </summary>
        public uint MaximumDurationMicroseconds
        {
            get { return _definition.MaximumDurationMicroseconds; }
            set
            {
                if (_definition.MaximumDurationMicroseconds != value)
                {
                    _definition.MaximumDurationMicroseconds = value;
                    UpdateRange();
                }
            }
        }

        /// <summary>
        /// Period length expressed in microseconds.
        /// </summary>
        public uint PeriodMicroseconds
        {
            get { return _definition.PeriodMicroseconds; }
            set
            {
                _definition.PeriodMicroseconds = value;
                if (!IsMoving)
                    Rotate();
            }
        }


    }
}
