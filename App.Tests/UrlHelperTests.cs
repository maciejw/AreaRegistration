using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace App.Tests
{
    public class UrlHelperTests
    {
        private UrlHelper CreateUrlHelper(string requestPath)
        {
            string expectedUrl = "{controller}/{action}/{id}";
            RouteValueDictionary expectedDefaults = new RouteValueDictionary()
            {
                { RouteConstants.controller, "Home" },
                { RouteConstants.action, "Index" },
                { RouteConstants.id, UrlParameter.Optional },
            };

            string[] expectedNamespaces = new[] { "Some.Namespace" };

            Assembly assembly = Assembly.GetExecutingAssembly();

            string applicationDefaultNamespace = "Some";

            var routeCollection = new RouteCollection();

            var area = new SomeAreaRegistration();

            area.RegisterArea(new AreaRegistrationContext(area.AreaName, routeCollection));

            Route route = routeCollection.RegisterDefaultRoutes(expectedUrl, expectedDefaults, expectedNamespaces, assembly, applicationDefaultNamespace);

            var testHttpContext = new TestHttpContext(new WebTestContext()
            {
                AppRelativeCurrentExecutionFilePath = requestPath,
            });

            var routeData = routeCollection.GetRouteData(testHttpContext);

            var requestContext = new RequestContext(testHttpContext, routeData);

            return new UrlHelper(requestContext, routeCollection);
        }

        [Fact]
        public void Should_return_propper_default_script_path()
        {
            var sut = CreateUrlHelper("~/");

            var path = sut.DefaultScript("file");

            Assert.Equal("/Scripts/file", path);
        }
        [Fact]

        public void Should_throw_exception_whtn_route_is_not_found()
        {
            var sut = CreateUrlHelper("~/");

            Assert.Throws<MissingResourceRouteException>(() => sut.AreaScript("file"));
        }

        [Fact]
        public void Should_return_propper_default_content_path()
        {
            var sut = CreateUrlHelper("~/");

            var path = sut.DefaultContent("file");

            Assert.Equal("/Content/file", path);
        }
        [Fact]
        public void Should_return_propper_area_script_path()
        {
            var sut = CreateUrlHelper("~/Some");

            var path = sut.AreaScript("file");

            Assert.Equal("/Some/Scripts/file", path);
        }

        [Fact]
        public void Should_return_propper_area_content_path()
        {
            var sut = CreateUrlHelper("~/Some");

            var path = sut.AreaContent("file");

            Assert.Equal("/Some/Content/file", path);
        }

    }


}
