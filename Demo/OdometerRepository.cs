using System;
using System.Threading.Tasks;

namespace Demo
{
    public class OdometorRepository : IGetOdometerReading
    {
        private readonly Random _random;

        public OdometorRepository()
        {
            _random = new Random();
        }

        public Task<object> GetOdometerReadingAsync(string userName)
        {
            var value = _random.Next(1000, 10000);

            return Task.FromResult(new { Info = $"Odometer reading as of {DateTimeOffset.UtcNow} - {value}" } as object);
        }
    }
}