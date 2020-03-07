using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public interface IGetOdometerReading
    {
        Task<object> GetOdometerReadingAsync(string userName);
    }
}
