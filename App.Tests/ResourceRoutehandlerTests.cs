using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Xunit;

namespace App.Tests
{

    public class ResourceRouteHandlerTests
    {

        public class TestResourceRouteHandler : ResourceRouteHandler
        {
            public TestResourceRouteHandler()
                : base(typeof(TestResourceRouteHandler).Assembly, "App.Tests.TestFolder")
            {
            }

            public new ResourceHandler GetHttpHandler(RequestContext requestContext)
            {
                return base.GetHttpHandler(requestContext) as ResourceHandler;
            }
        }

        [Fact]
        public void Should_return_expected_handler()
        {
            var route = new Route("Scripts/{*file}", null);
            var testHttpContext = new TestHttpContext(new WebTestContext()
            {
                AppRelativeCurrentExecutionFilePath = "~/Scripts/file.js"
            });
            var routeData = route.GetRouteData(testHttpContext);
            var requestContext = new RequestContext(testHttpContext, routeData);


            var sut = new TestResourceRouteHandler();
            var httpHandler = sut.GetHttpHandler(requestContext);


            Assert.NotNull(httpHandler);

        }
    }
}
