namespace Sensors.Sensor
{
    using System;
    using Math = System.Math;
    using Microsoft.SPOT;
    using Microsoft.SPOT.Hardware;
    using SecretLabs.NETMF.Hardware.Netduino;
    using SecretLabs.NETMF.Hardware;

    public class Accelerometer : ISensor
    {
        /// <summary>
        /// Scale provided by Accelerometer documentation.
        /// </summary>
        private const double SCALE = -0.0157;
        
        private AnalogInput inputPort;
        

        public Accelerometer(Cpu.Pin pin)
        {
            inputPort = new AnalogInput(pin);
        }

        public void Calibrate(TimeSpan timespan)
        {
            var startTime = DateTime.UtcNow;
            long sum = 0;
            long counter = 0;
            while (DateTime.UtcNow - startTime < timespan)
            {
                sum += Value;
                counter++;
            }

            var calculatedCenter = (int)System.Math.Round(sum / counter);

            var min = -calculatedCenter;
            var max = 1024 - calculatedCenter;

            inputPort.SetRange(min, max);

                
        }
        
        /// <summary>
        /// Returns the angular tilt in Radians. Rather than using expensive trigonometric functions, we'll use an approximation
        /// that works up until 30 degrees (value*scale).
        /// </summary>
        public double Radians
        {
            get
            {
                return (Value * SCALE);
            }
        }

        /// <summary>
        /// Returns the angular tilt in Degrees
        /// </summary>
        public double Degrees
        {
            get
            {
                return Radians * Constants.RAD2DEG_COF;
            }
        }


        public int Value 
        { 
            get 
            {
                double sum = 0;
                for (int x = 0; x < 10; x++) 
                {
                    sum += inputPort.Read();
                }

                int value = (int)Math.Round((sum / 10));
                
                return value;
            } 
        }
    }
}
