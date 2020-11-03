using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Bindings;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Functions.CustomBindings
{
    public class AccessTokenBindingProvider : IBindingProvider
    {
        private readonly INameResolver _nameResolver;

        public AccessTokenBindingProvider(INameResolver nameResolver)
        {
            _nameResolver = nameResolver ?? throw new ArgumentNullException(nameof(nameResolver));
        }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            var attribute = context.Parameter.GetCustomAttribute<AccessTokenAttribute>(inherit: false);

            if (attribute == null)
            {
                return Task.FromResult<IBinding>(null);
            }

            IBinding binding = new AccessTokenBinding(attribute, _nameResolver);

            return Task.FromResult(binding);
        }
    }
}
