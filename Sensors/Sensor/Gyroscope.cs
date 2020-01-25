namespace Sensors.Sensor
{
    using System;
    using Math = System.Math;
    using Microsoft.SPOT;
    using Microsoft.SPOT.Hardware;
    using SecretLabs.NETMF.Hardware;

    public class Gyroscope : ISensor
    {
        private const double SCALE = 0.03;
        private AnalogInput inputPort;

        /// <summary>
        /// Initializes a new instance of the gyroscope class, using the provided
        /// pin. The supplied voltage will be used to calculate a 0-reading.
        /// </summary>
        /// <param name="pin">The analog input pin that connects to a gyroscope axis.</param>
        public Gyroscope(Cpu.Pin pin)
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

        public double Radians
        {
            get
            {
                return (Value * SCALE);
            }
        }

        /// <summary>
        /// Returns the angular tilt
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
