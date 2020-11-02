using Microsoft.Azure.WebJobs.Description;
using System;

namespace Functions.CustomBindings
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class AccessTokenAttribute : Attribute { }
}
