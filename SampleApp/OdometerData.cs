using System;

namespace SampleApp
{
    public class OdometerData
    {
        public int Value { get; set; }

        public DateTimeOffset Date { get; set; }

        public string Message { get; set; }
    }
}