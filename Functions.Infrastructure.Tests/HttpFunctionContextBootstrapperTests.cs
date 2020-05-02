using Xunit;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Numaka.Functions.Infrastructure.Tests
{
    public class HttpFunctionContextBootstrapperTests
    {
        [Fact]
        public void Bootstrap_With_Valid_Inputs_Should_Have_A_HttpFunctionContext()
        {
            var httpFunctionContextFactory = new HttpFunctionContextBootstrapper();

            var httpFunctionContext = httpFunctionContextFactory.Bootstrap(Substitute.For<HttpRequest>(), Substitute.For<ILogger>());

            Assert.NotNull(httpFunctionContext);
        }

        [Fact]
        public void Bootstrap_With_Valid_Inputs_Should_Have_A_Request()
        {
            var httpFunctionContextFactory = new HttpFunctionContextBootstrapper();

            var httpFunctionContext = httpFunctionContextFactory.Bootstrap(Substitute.For<HttpRequest>(), Substitute.For<ILogger>());

            Assert.NotNull(httpFunctionContext.Request);
        }

        [Fact]
        public void Bootstrap_With_Valid_Inputs_Should_Have_A_Logger()
        {
            var httpFunctionContextFactory = new HttpFunctionContextBootstrapper();

            var httpFunctionContext = httpFunctionContextFactory.Bootstrap(Substitute.For<HttpRequest>(), Substitute.For<ILogger>());

            Assert.NotNull(httpFunctionContext.Logger);
        }

        [Fact]
        public void Bootstrap_With_Valid_Inputs_Should_Have_A_Null_ActionResult()
        {
            var httpFunctionContextFactory = new HttpFunctionContextBootstrapper();

            var httpFunctionContext = httpFunctionContextFactory.Bootstrap(Substitute.For<HttpRequest>(), Substitute.For<ILogger>());

            Assert.Null(httpFunctionContext.ActionResult);
        }

        [Fact]
        public void Bootstrap_With_Valid_Inputs_Should_Have_A_Null_ClaimsPrincipal()
        {
            var httpFunctionContextFactory = new HttpFunctionContextBootstrapper();

            var httpFunctionContext = httpFunctionContextFactory.Bootstrap(Substitute.For<HttpRequest>(), Substitute.For<ILogger>());

            Assert.Null(httpFunctionContext.ClaimsPrincipal);
        }
    }
}
