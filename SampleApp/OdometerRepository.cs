using System;
using System.Threading.Tasks;

namespace SampleApp
{
    public class OdometorRepository : IGetOdometerReading
    {
        private readonly Random _random;

        public OdometorRepository()
        {
            _random = new Random();
        }

        public Task<OdometerData> GetOdometerReadingAsync(string userName)
        {
            var value = _random.Next(10000, 20000);

            var data = new OdometerData { Value = value, Date = DateTimeOffset.UtcNow, Message = $"Odometer readings for {userName}" };

            return Task.FromResult(data);
        }
    }
}