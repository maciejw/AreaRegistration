using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorGenerator.Mvc;
using Xunit;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web;
using System.Collections;
using System.IO;
using System.Web.WebPages;
using System.Diagnostics.CodeAnalysis;

namespace App.Tests
{
    public class ChangingAreaCompositePrecompiledMvcEngineTests
    {
        [ExcludeFromCodeCoverage]
        [PageVirtualPath("~/Areas/AreaName/Views/Test/TestView.cshtml")]
        public partial class AreaTestView : System.Web.Mvc.WebViewPage<dynamic>
        {
            public override void Execute() { }
        }
        [ExcludeFromCodeCoverage]
        [PageVirtualPath("~/Views/Test/TestView.cshtml")]
        public partial class TestView : System.Web.Mvc.WebViewPage<dynamic>
        {
            public override void Execute() { }
        }
        [ExcludeFromCodeCoverage]
        public class TestController : Controller { }


        private static ControllerContext CreateControllerContext(string area = null, string areaFolder = null)
        {
            var httpContext = new TestHttpContext(new WebTestContext { AppRelativeCurrentExecutionFilePath = $"~/{area}" });

            var route = new Route(area, null)
            {
                Defaults = new RouteValueDictionary
                {
                    { "controller", "Test" },
                },
                DataTokens = new RouteValueDictionary()
            };
            if (area != null)
            {
                route.DataTokens.Add("area", area);
            }
            if (areaFolder != null)
            {
                route.DataTokens.Add("areaFolder", areaFolder);
            }

            return new ControllerContext(httpContext, route.GetRouteData(httpContext), new TestController());
        }


        private readonly ChangingAreaCompositePrecompiledMvcEngine sut;

        public ChangingAreaCompositePrecompiledMvcEngineTests()
        {
            HttpContextLocator.HttpContextFactory = () => new TestHttpContext(new WebTestContext());

            App.RazorGeneratorMvcStart.Start();

            sut = ViewEngines.Engines.First() as ChangingAreaCompositePrecompiledMvcEngine;

        }
        [ExcludeFromCodeCoverage]
        public static IEnumerable<object[]> AreaTestData()
        {
            yield return new object[] { null, null };
            yield return new object[] { "AreaName", null };
            yield return new object[] { "DifferentAreaNameWhenFolder", "AreaName" };
        }

        [Theory]
        [MemberData("AreaTestData")]
        public void Should_be_able_to_find_view(string area, string areaFolder)
        {
            ControllerContext controllerContext = CreateControllerContext(area, areaFolder);

            var viewResult = this.sut.FindView(controllerContext, "TestView", "", false);

            Assert.NotNull(viewResult);

            Assert.NotNull(viewResult.View);
        }

        [Theory]
        [MemberData("AreaTestData")]
        public void Should_be_able_to_find_partial_view(string area, string areaFolder)
        {
            ControllerContext controllerContext = CreateControllerContext(area, areaFolder);

            var viewResult = this.sut.FindPartialView(controllerContext, "TestView", false);

            Assert.NotNull(viewResult);

            Assert.NotNull(viewResult.View);
        }

        [Fact]
        public void Should_be_able_to_release_view()
        {
            ControllerContext controllerContext = CreateControllerContext();

            var viewResult = this.sut.FindView(controllerContext, "TestView", "", false);

            this.sut.ReleaseView(controllerContext, viewResult.View);
        }

    }
}
