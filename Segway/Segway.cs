namespace Segway
{
    using System;
    using Microsoft.SPOT;
    using Controllers;
using Sensors.Sensor;
using Microsoft.SPOT.Hardware;
    using System.Threading;

    public class Segway
    {
        private MotorController leftEngine;
        private MotorController rightEngine;

        private Accelerometer accelerometer;
        private Gyroscope gyroscope;

        private PIDController pidController;

        public Segway(Cpu.Pin accelerometerPin, Cpu.Pin gyroscopePin, Cpu.Pin engineLeftPin, Cpu.Pin engineRightPin)
        {
            accelerometer = new Accelerometer(accelerometerPin);
            gyroscope = new Gyroscope(gyroscopePin);
            
            pidController = new PIDController(gyroscope, accelerometer);
            
            rightEngine = new MotorController(engineRightPin);
            leftEngine = new MotorController(engineLeftPin);
        }

        public void Initalize()
        {
            gyroscope.Calibrate(TimeSpan.FromTicks(TimeSpan.TicksPerSecond));
            accelerometer.Calibrate(TimeSpan.FromTicks(TimeSpan.TicksPerSecond));
            pidController.StartSample(50);
        }

        public void Run(int correctiveFrequency) 
        {
            var period = (1000 / 10);
            DateTime preTime;
            
            while (true) 
            {
                preTime = DateTime.UtcNow;
                
                var output = pidController.Output;
                Debug.Print("Output: " + output);


                leftEngine.Power(output);
                rightEngine.Power(output);
                
                var sleepTime = period - (DateTime.UtcNow - preTime).Milliseconds;
                
                if (sleepTime > 0)
                {
                    Thread.Sleep(sleepTime);
                }
                
            }
        }



    }
}
