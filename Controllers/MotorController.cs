
namespace Controllers
{
    using System;
    using Microsoft.SPOT;
    using Microsoft.SPOT.Hardware;
    using SecretLabs.NETMF.Hardware.Netduino;
    using SecretLabs.NETMF.Hardware;
    using System.IO.Ports;
    using SystemMath = System.Math;

    public class MotorController
    {
        //D5, D6, D9 and D10.
        PWM output;
        const int ZERO_POWER = 0;
        const int MAX_REVERSE = -500;
        const int MAX_FORWARD = 500;
        
        // The period of the PWM signal. RC Servo signals are usually updated every
        // 20 ms.
        const int PERIOD = 20000;

        const int CENTER_PULSE_WIDTH = 1500;
        
        public MotorController(Cpu.Pin pin) 
        {
            output = new PWM(pin);
            Power(0);
        }

        /// <summary>
        /// Set the speed of the motor. The motor will accept a speed between -500 and 500.
        /// -500 is full reversal, 0 stand still and 500 full forward.
        /// </summary>
        /// <param name="speed">The speed to apply to the engine.</param>
        public void Power(int speed) 
        {
            uint pulseWidth = MapSpeed(speed);
            output.SetPulse(PERIOD, pulseWidth);
        }

        private uint MapSpeed(int speed) 
        {
            speed = SystemMath.Min(MAX_FORWARD, speed);
            speed = SystemMath.Max(MAX_REVERSE, speed);
            speed += CENTER_PULSE_WIDTH;
            return (uint)speed;
        }
    }
}
