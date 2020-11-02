using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using System;

namespace Functions.CustomBindings
{
    [Extension("AccessToken")]
    public class AccessTokenExtensionProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var provider = new AccessTokenBindingProvider();

            context.AddBindingRule<AccessTokenAttribute>().Bind(provider);
        }
    }
}
