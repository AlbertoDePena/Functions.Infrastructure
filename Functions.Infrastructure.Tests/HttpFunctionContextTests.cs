using System;
using Xunit;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Numaka.Functions.Infrastructure.Tests
{
    public class HttpFunctionContextTests
    {
        [Fact]
        public void Constructor_With_Null_Request_Should_Throw_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpFunctionContext(null, Substitute.For<ILogger>()));
        }

        [Fact]
        public void Constructor_With_Null_Logger_Should_Throw_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpFunctionContext(Substitute.For<HttpRequest>(), null));
        }
    }
}
