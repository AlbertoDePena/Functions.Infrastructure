using System;

namespace SampleApp
{
    public class OdometerData
    {
        public double Value { get; set; }

        public string VIN { get; set; }

        public DateTimeOffset Date { get; set; }
    }
}