using System.Threading.Tasks;

namespace SampleApp
{
    public interface IGetOdometerReading
    {
        Task<OdometerData> GetOdometerReadingAsync(string vin);
    }
}
