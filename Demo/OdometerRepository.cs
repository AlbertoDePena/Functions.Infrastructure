using System.Threading.Tasks;

namespace Demo
{
    public class OdometorRepository : IGetOdometerUsingRegoQuery
    {
        public Task<object> ExecuteAsync(string userName)
        {
            return Task.FromResult(new { Name = userName } as object);
        }
    }
}