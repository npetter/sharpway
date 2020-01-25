namespace Sensors
{
    using System;

    public interface ISensor
    {
        double Radians { get; }

        double Degrees { get; }

        void Calibrate(TimeSpan timespan);
    }
}
