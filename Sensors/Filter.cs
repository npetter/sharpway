using System;
using Microsoft.SPOT;

namespace Sensors
{
    public class Filter
    {
        private ISensor analogSensor;
        private double oldValue;
        private double oldWeight;
        private double newWeight;
        
        public Filter(ISensor sensor, double filterCoefficient) 
        {
            analogSensor = sensor;
            oldWeight = filterCoefficient;
            newWeight = 1-filterCoefficient;
            oldValue = 0;
        }

        public double Value 
        { 
            get 
            {
                oldValue = (oldValue * oldWeight) + (analogSensor.Degrees * newWeight);
                return oldValue;
            } 
        }
    }
}
