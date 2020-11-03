using Microsoft.Azure.WebJobs.Description;
using System;

namespace Functions.CustomBindings
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class AccessTokenAttribute : Attribute
    {
        public AccessTokenAttribute(string b2cClientId, string b2cMetadataAddress, string aadClientId, string aadMetadataAddress)
        {
            if (string.IsNullOrWhiteSpace(b2cClientId)) throw new ArgumentNullException(nameof(b2cClientId));
            if (string.IsNullOrWhiteSpace(b2cMetadataAddress)) throw new ArgumentNullException(nameof(b2cMetadataAddress));
            if (string.IsNullOrWhiteSpace(aadClientId)) throw new ArgumentNullException(nameof(aadClientId));
            if (string.IsNullOrWhiteSpace(aadMetadataAddress)) throw new ArgumentNullException(nameof(aadMetadataAddress));

            B2cClientId = b2cClientId;
            B2cMetadataAddress = b2cMetadataAddress;
            AadClientId = aadClientId;
            AadMetadataAddress = aadMetadataAddress;
        }

        [AutoResolve]
        public string B2cClientId { get; }

        [AutoResolve]
        public string B2cMetadataAddress { get; }

        [AutoResolve]
        public string AadClientId { get; }

        [AutoResolve]
        public string AadMetadataAddress { get; }
    }
}
