using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;
using static App.ResourceRouteHandler;

namespace App.Tests
{
    public class SomeAreaRegistration : AppAreaRegistration
    {
        public override string AreaBaseNamespace => "App.Areas";

        public override string AreaName => "Some";
    }

    public class RouteRegistrationTests
    {
        string defaultScriptResourceRouteName = GetDefaultResourceRouteName(DefaultFolders.Scripts);
        string defaultContentResourceRouteName = GetDefaultResourceRouteName(DefaultFolders.Content);

        const string expectedUrl = "{controller}/{action}/{id}";


        RouteValueDictionary expectedDefaults = new RouteValueDictionary()
        {
            { RouteConstants.controller, "Home" },
            { RouteConstants.action, "Index" },
            { RouteConstants.id, UrlParameter.Optional },
        };

        string[] expectedNamespaces = new[] { "Some.Namespace" };

        Assembly assembly = Assembly.GetExecutingAssembly();
        const string applicationDefaultNamespace = "Some";


        private readonly RouteCollection sut;

        public RouteRegistrationTests()
        {
            sut = new RouteCollection();
        }


        [Fact]
        public void Should_register_default_routes_with_areas_registered_first()
        {
            var area = new SomeAreaRegistration();

            area.RegisterArea(new AreaRegistrationContext(area.AreaName, sut));

            Route route = sut.RegisterDefaultRoutes(expectedUrl, expectedDefaults, expectedNamespaces, assembly, applicationDefaultNamespace);

            Assert.Equal(6, sut.Count);

            var defaultAreaRoute = sut[AppAreaRegistration.GetAreaDefaultRouteName(area.AreaName)] as Route;

            Assert.NotNull(defaultAreaRoute);

            Assert.Equal($"{area.AreaName}/{expectedUrl}", defaultAreaRoute.Url);

            AssertRouteDefaults(defaultAreaRoute, RouteConstants.controller, expectedDefaults[RouteConstants.controller]);
            AssertRouteDefaults(defaultAreaRoute, RouteConstants.action, expectedDefaults[RouteConstants.action]);
            AssertRouteDefaults(defaultAreaRoute, RouteConstants.id, expectedDefaults[RouteConstants.id]);

            object areaScriptResourceRouteName = GetAreaResourceRouteName(area.AreaName, DefaultFolders.Scripts);
            object areaContentResourceRouteName = GetAreaResourceRouteName(area.AreaName, DefaultFolders.Content);

            AssertDataToken(defaultAreaRoute, AppAreaRegistration.DataTokens.areaFolder, area.AreaFolder);
            AssertDataToken(defaultAreaRoute, AppAreaRegistration.DataTokens.areaDefaultRouteName, AppAreaRegistration.GetAreaDefaultRouteName(area.AreaName));

            AssertDataToken(defaultAreaRoute, DataTokens.areaScriptResourceRouteName, areaScriptResourceRouteName);
            AssertDataToken(defaultAreaRoute, DataTokens.areaContentResourceRouteName, areaContentResourceRouteName);

            AssertDataToken(defaultAreaRoute, DataTokens.defaultScriptResourceRouteName, defaultScriptResourceRouteName);
            AssertDataToken(defaultAreaRoute, DataTokens.defaultContentResourceRouteName, defaultContentResourceRouteName);

            AssertRouteExcludeConstraint(defaultAreaRoute);

            AssertResourceRoute(defaultScriptResourceRouteName);
            AssertResourceRoute(defaultContentResourceRouteName);


        }

        [Fact]
        public void Should_register_default_routes_without_areas_registered_first()
        {

            Route route = sut.RegisterDefaultRoutes(expectedUrl, expectedDefaults, expectedNamespaces, assembly, applicationDefaultNamespace);

            Assert.True(sut.RouteExistingFiles);

            Assert.Equal(3, sut.Count);

            var defaultRoute = sut[RouteCollectionExtensions.@default] as Route;

            Assert.NotNull(defaultRoute);

            Assert.Same(defaultRoute, route);

            Assert.Equal(expectedUrl, defaultRoute.Url);

            AssertRouteDefaults(defaultRoute, RouteConstants.controller, expectedDefaults[RouteConstants.controller]);
            AssertRouteDefaults(defaultRoute, RouteConstants.action, expectedDefaults[RouteConstants.action]);
            AssertRouteDefaults(defaultRoute, RouteConstants.id, expectedDefaults[RouteConstants.id]);

            AssertDataToken(defaultRoute, "Namespaces", expectedNamespaces);

            AssertDataToken(defaultRoute, DataTokens.defaultScriptResourceRouteName, defaultScriptResourceRouteName);
            AssertDataToken(defaultRoute, DataTokens.defaultContentResourceRouteName, defaultContentResourceRouteName);

            AssertRouteExcludeConstraint(defaultRoute);

            AssertResourceRoute(defaultScriptResourceRouteName);
            AssertResourceRoute(defaultContentResourceRouteName);
        }

        private void AssertRouteDefaults(Route route, string routeDefaultName, object routeDefault)
        {
            Assert.True(route.Defaults.ContainsKey(routeDefaultName));
            Assert.Equal(routeDefault, route.Defaults[routeDefaultName]);
        }

        private void AssertDataToken(Route defaultRoute, string dataTokenName, object dataTokenValue)
        {
            Assert.True(defaultRoute.DataTokens.ContainsKey(dataTokenName));
            Assert.Equal(dataTokenValue, defaultRoute.DataTokens[dataTokenName]);
        }

        private static void AssertRouteExcludeConstraint(Route route)
        {
            Assert.True(route.Constraints.ContainsKey(RouteConstants.controller));
            Assert.IsType<ExcludeRouteConstraint>(route.Constraints[RouteConstants.controller]);
        }

        private void AssertResourceRoute(string routeName)
        {
            var resourceRoute = sut[routeName] as Route;

            Assert.NotNull(resourceRoute);
        }
    }
}
