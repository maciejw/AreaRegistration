using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics.Contracts;

namespace App
{
    public class ResourceRouteHandler : MvcRouteHandler
    {
        private readonly Assembly resourceAssembly;
        private readonly string baseNamespace;

        public ResourceRouteHandler(Assembly resourceAssembly, string baseNamespace)
        {
            this.resourceAssembly = resourceAssembly;
            this.baseNamespace = baseNamespace;
        }

        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ResourceHandler(requestContext, new ResourceFileContentProvider(resourceAssembly, baseNamespace));
        }

        public static class RouteConstants
        {
            public const string path = nameof(path);
        }

        public static Route RegisterResourceRoute(Func<string, string, Route> routeFactory, string resourcePrefix, Assembly resourceAssembly, string baseNamespace)
        {
            Contract.Requires(routeFactory != null, "routeFactory is null.");
            Contract.Requires(!string.IsNullOrEmpty(resourcePrefix), "resourcePrefix is null or empty.");
            Contract.Requires(resourceAssembly != null, "resourceAssembly is null.");
            Contract.Requires(!string.IsNullOrEmpty(baseNamespace), "baseNamespace is null or empty.");

            Route route = RegisterRoute(routeFactory, resourceAssembly, baseNamespace,
                 $"Default_{resourcePrefix}_resource",
                 $"{resourcePrefix}/{{*{RouteConstants.path}}}");

            return route;
        }
        public static Route RegisterResourceRoute(Func<string, string, Route> routeFactory, string areaName, string resourcePrefix, Assembly resourceAssembly, string baseNamespace)
        {
            Contract.Requires(routeFactory != null, "routeFactory is null.");
            Contract.Requires(!string.IsNullOrEmpty(areaName), "areaName is null or empty.");
            Contract.Requires(!string.IsNullOrEmpty(resourcePrefix), "resourcePrefix is null or empty.");
            Contract.Requires(resourceAssembly != null, "resourceAssembly is null.");
            Contract.Requires(!string.IsNullOrEmpty(baseNamespace), "baseNamespace is null or empty.");



            Route route = RegisterRoute(routeFactory, resourceAssembly, baseNamespace,
                $"{areaName}_{resourcePrefix}_resource",
                $"{areaName}/{resourcePrefix}/{{*{RouteConstants.path}}}");

            return route;
        }

        private static Route RegisterRoute(Func<string, string, Route> routeFactory, Assembly resourceAssembly, string baseNamespace, string name, string url)
        {
            Route route = routeFactory(
                  name,
                  url
                );
            route.RouteHandler = new ResourceRouteHandler(resourceAssembly, baseNamespace);
            return route;
        }
    }

}
