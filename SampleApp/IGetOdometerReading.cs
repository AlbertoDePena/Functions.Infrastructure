using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
    public interface IGetOdometerReading
    {
        Task<OdometerData> GetOdometerReadingAsync(string userName);
    }
}
