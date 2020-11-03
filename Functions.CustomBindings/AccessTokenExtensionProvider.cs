using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using System;

namespace Functions.CustomBindings
{
    [Extension("AccessToken")]
    public class AccessTokenExtensionProvider : IExtensionConfigProvider
    {
        private readonly INameResolver _nameResolver;

        public AccessTokenExtensionProvider(INameResolver nameResolver)
        {
            _nameResolver = nameResolver ?? throw new ArgumentNullException(nameof(nameResolver));
        }

        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var provider = new AccessTokenBindingProvider(_nameResolver);

            context.AddBindingRule<AccessTokenAttribute>().Bind(provider);
        }
    }
}
