using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public interface IGetOdometerUsingRegoQuery
    {
        Task<object> ExecuteAsync(string userName);
    }
}
