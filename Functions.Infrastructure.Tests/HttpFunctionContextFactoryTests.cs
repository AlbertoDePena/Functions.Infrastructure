using Xunit;
using NSubstitute;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Functions.Infrastructure.Contracts;

namespace Functions.Infrastructure.Tests
{
    public class HttpFunctionContextFactoryTests
    {
        [Fact]
        public void Bootstrap_With_Valid_Inputs_Should_Have_A_HttpFunctionContext()
        {
            var httpFunctionContextFactory = new HttpFunctionContextFactory();

            var httpFunctionContext = httpFunctionContextFactory.Bootstrap(Substitute.For<HttpRequestMessage>(), Substitute.For<ILogger>());

            Assert.NotNull(httpFunctionContext);
        }

        [Fact]
        public void Bootstrap_With_Valid_Inputs_Should_Have_A_Request()
        {
            var httpFunctionContextFactory = new HttpFunctionContextFactory();

            var httpFunctionContext = httpFunctionContextFactory.Bootstrap(Substitute.For<HttpRequestMessage>(), Substitute.For<ILogger>());

            Assert.NotNull(httpFunctionContext.Request);
        }

        [Fact]
        public void Bootstrap_With_Valid_Inputs_Should_Have_A_Logger()
        {
            var httpFunctionContextFactory = new HttpFunctionContextFactory();

            var httpFunctionContext = httpFunctionContextFactory.Bootstrap(Substitute.For<HttpRequestMessage>(), Substitute.For<ILogger>());

            Assert.NotNull(httpFunctionContext.Logger);
        }

        [Fact]
        public void Bootstrap_With_Valid_Inputs_Should_Have_A_Null_Response()
        {
            var httpFunctionContextFactory = new HttpFunctionContextFactory();

            var httpFunctionContext = httpFunctionContextFactory.Bootstrap(Substitute.For<HttpRequestMessage>(), Substitute.For<ILogger>());

            Assert.Null(httpFunctionContext.Response);
        }

        [Fact]
        public void Bootstrap_With_Valid_Inputs_Should_Have_A_Null_User()
        {
            var httpFunctionContextFactory = new HttpFunctionContextFactory();

            var httpFunctionContext = httpFunctionContextFactory.Bootstrap(Substitute.For<HttpRequestMessage>(), Substitute.For<ILogger>());

            Assert.Null(httpFunctionContext.User);
        }
    }
}
