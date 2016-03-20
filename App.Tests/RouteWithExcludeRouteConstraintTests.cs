using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Xunit;

namespace App.Tests
{

    public class RouteWithExcludeRouteConstraintTests
    {
        private readonly Route sut;

        public RouteWithExcludeRouteConstraintTests()
        {
            sut = new Route("{controller}", null);
            sut.Constraints = new RouteValueDictionary(ResourceRouteHandler.CreateControllerExcludeConstraintForDefaultFolders());
        }

        [Theory]
        [InlineData("NotExcluded", "NotExcluded")]
        [InlineData("Scripts", null)]
        [InlineData("Content", null)]
        public void Should_not_use_default_resource_folders_as_controllers(string path, string controller)
        {
            var testHttpContext = new TestHttpContext(new WebTestContext()
            {
                AppRelativeCurrentExecutionFilePath = "~/" + path
            });

            var routeData = sut.GetRouteData(testHttpContext);

            Assert.Equal(controller, routeData?.Values["controller"] as string);
        }
    }
}
