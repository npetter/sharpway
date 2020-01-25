using System;
using Microsoft.SPOT;

namespace Sensors.Sensor
{
    public class Integrator : ISensor
    {
        ISensor sensor;
        
        # region Integration related
        
        double integral;
        DateTime lastTime;
        
        # endregion


        public Integrator(ISensor sensor) 
        {
            this.sensor = sensor;
            lastTime = DateTime.MinValue;
        }
        public double Radians
        {
            get 
            {
                var now = DateTime.UtcNow;
                
                if (lastTime == DateTime.MinValue)
                {
                    lastTime = now;
                }
                
                var currentReading = sensor.Radians;
                double intervalDuration = (now - lastTime).Milliseconds / 1000;
                var subSum = currentReading * intervalDuration;
                integral += subSum;

                lastTime = now;

                return integral;
            }
        }

        public double Degrees
        {
            get { return Radians * Constants.RAD2DEG_COF; }
        }

        public void Calibrate(TimeSpan timespan)
        {
            sensor.Calibrate(timespan);
        }
    }
}
