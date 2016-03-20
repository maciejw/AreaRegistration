using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App
{
    public static class RouteCollectionExtensions
    {
        public const string @default = nameof(@default);

        public static Route RegisterDefaultRoutes(this RouteCollection @this, string url, object defaults, string[] namespaces, Assembly resourceAssembly, string applicationDefaultNamespace)
        {
            Contract.Requires(@this != null);

            @this.RouteExistingFiles = true;

            var defaultRouteName = @default;
            var defaultScriptResourceRouteName = ResourceRouteHandler.GetDefaultResourceRouteName(ResourceRouteHandler.DefaultFolders.Scripts);
            var defaultContentResourceRouteName = ResourceRouteHandler.GetDefaultResourceRouteName(ResourceRouteHandler.DefaultFolders.Content);

            var constraints = ResourceRouteHandler.CreateControllerExcludeConstraintForDefaultFolders();

            var defaultRoute = @this.MapRoute(defaultRouteName, url, defaults, constraints, namespaces);

            defaultRoute.DataTokens[ResourceRouteHandler.DataTokens.defaultScriptResourceRouteName] = defaultScriptResourceRouteName;
            defaultRoute.DataTokens[ResourceRouteHandler.DataTokens.defaultContentResourceRouteName] = defaultContentResourceRouteName;


            var areaRoutes = @this
                .OfType<Route>()
                .Where(r => r.IsAreaDefaultRoute());

            foreach (var areaRoute in areaRoutes)
            {
                areaRoute.DataTokens[ResourceRouteHandler.DataTokens.defaultScriptResourceRouteName] = defaultScriptResourceRouteName;
                areaRoute.DataTokens[ResourceRouteHandler.DataTokens.defaultContentResourceRouteName] = defaultContentResourceRouteName;
                areaRoute.DataTokens[DefaultRouteConstants.defaultRouteName] = defaultRouteName;
            }

            ResourceRouteHandler.RegisterResourceRoute(@this.MapRoute, defaultScriptResourceRouteName, ResourceRouteHandler.DefaultFolders.Scripts, resourceAssembly, applicationDefaultNamespace);
            ResourceRouteHandler.RegisterResourceRoute(@this.MapRoute, defaultContentResourceRouteName, ResourceRouteHandler.DefaultFolders.Content, resourceAssembly, applicationDefaultNamespace);

            return defaultRoute;
        }
    }
}
