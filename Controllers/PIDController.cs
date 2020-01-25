namespace Controllers
{
    using System;
    using Microsoft.SPOT;
    using Sensors;
    using Sensors.Sensor;
    using System.Threading;
    using System.Runtime.CompilerServices;

    public class PIDController
    {
        DateTime lastTime;
        
        double integral;

        object lockObject = new object();

        Timer sampleTimer;

        private const double LOWPASS_COF = 0.98;
        private const double HIGHPASS_COF = 0.02;
        private const double Kp = 0.4;
        private const double Kd = 0.6;
        
        
        Filter lowpassFilter;
        Filter highpassFilter;
        Gyroscope gyroscope;


        public PIDController(Gyroscope gyro, Accelerometer acc) 
        {
            lowpassFilter = new Filter(acc, LOWPASS_COF);

            highpassFilter = new Filter(new Integrator(gyro), HIGHPASS_COF);
            
            gyroscope = gyro;
            lastTime = DateTime.MinValue;

            
        }

        public void StartSample(int sampleFrequency) 
        {
            var periodMs = 1000 / sampleFrequency;
            var dueTime = new TimeSpan(0, 0, 0, 0, 0);
            var period = new TimeSpan(0, 0, 0, 0, periodMs);
            if (sampleTimer == null)
            {
                //TimerCallback sampleMethod = new TimerCallback(this.Sample);
                sampleTimer = new Timer(Sample, null, dueTime, period);
            }
            else
            {
                sampleTimer.Change(dueTime, period);
            }
        }


        public double Derivative 
        { 
            get 
            { 
                return gyroscope.Degrees; 
            } 
        }

        public double Proportional 
        { 
            get 
            { 
                return lowpassFilter.Value + highpassFilter.Value;
            }
        }

        public void Sample(Object stateInfo) 
        {
            var now = DateTime.UtcNow;

            if (lastTime == DateTime.MinValue)
            {
                lastTime = now;
            }

            // Variable of integration
            double dx = (Kp * Proportional) + (Kd * Derivative);
            
            // Integral interval
            double dt = ((double)(now - lastTime).Milliseconds) / 1000;
            
            var subIntegral = dx * dt;

            lock (lockObject)
            {
                integral += subIntegral;
            }
            
            lastTime = now;
        }

        
        public int Output 
        { 
            get 
            {
                lock (lockObject)
                {
                    return (int)integral;
                }
            } 
        }
    }
}
