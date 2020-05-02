using System;
using System.Threading.Tasks;

namespace SampleApp
{
    public class OdometorRepository : IGetOdometerReading
    {
        private readonly Random _random;

        public OdometorRepository() => _random = new Random();

        public Task<OdometerData> GetOdometerReadingAsync(string vin)
        {
            var value = GetRandomNumber(0, 200000);

            var data = new OdometerData { VIN = vin, Value = value, Date = DateTimeOffset.UtcNow };

            return Task.FromResult(data);
        }

        private double GetRandomNumber(double minimum, double maximum) => (_random.NextDouble() * (maximum - minimum)) + minimum;
    }
}