﻿using Microsoft.Azure.WebJobs.Host.Bindings;
using System.Threading.Tasks;

namespace Functions.CustomBindings
{
    public class AccessTokenBindingProvider : IBindingProvider
    {
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            IBinding binding = new AccessTokenBinding();

            return Task.FromResult(binding);
        }
    }
}