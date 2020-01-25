namespace Segway
{
    using System;
    using System.Threading;
    using Microsoft.SPOT;
    using Microsoft.SPOT.Hardware;
    using SecretLabs.NETMF.Hardware;
    using SecretLabs.NETMF.Hardware.Netduino;
    using Controllers;
    using Sensors.Sensor;
    using Sensors;

    public class Program
    {
        private const Cpu.Pin ACCELEROMETER = Pins.GPIO_PIN_A1;
        private const Cpu.Pin GYROSCOPE = Pins.GPIO_PIN_A2;
        private const Cpu.Pin ENGINE_LEFT = Pins.GPIO_PIN_D9;
        private const Cpu.Pin ENGINE_RIGHT = Pins.GPIO_PIN_D10;

        public static void Main()
        {
            var segway = new Segway(ACCELEROMETER, GYROSCOPE, ENGINE_LEFT, ENGINE_RIGHT);
            segway.Initalize();
            segway.Run(100);
        }
    }
}
