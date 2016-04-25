using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics.Contracts;
using System.IO;

namespace App
{
    public class ResourceRouteHandler : MvcRouteHandler
    {
        public static class DefaultFolders
        {
            public const string Content = nameof(Content);
            public const string Scripts = nameof(Scripts);
        }
        public static class DataTokens
        {
            public const string areaScriptResourceRouteName = nameof(areaScriptResourceRouteName);
            public const string defaultScriptResourceRouteName = nameof(defaultScriptResourceRouteName);

            public const string areaContentResourceRouteName = nameof(areaContentResourceRouteName);
            public const string defaultContentResourceRouteName = nameof(defaultContentResourceRouteName);
        }
        public static class RouteConstants
        {
            public const string file = nameof(file);
        }

        public static object CreateControllerExcludeConstraintForDefaultFolders()
        {
            return new { controller = new ExcludeRouteConstraint(DefaultFolders.Scripts, DefaultFolders.Content) };
        }

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

        public static string GetDefaultResourceRouteName(string resourceFolder)
        {
            return $"{resourceFolder}_resource";
        }

        public static string GetAreaResourceRouteName(string areaName, string resourceFolder)
        {
            return $"{areaName}_{resourceFolder}_resource";
        }

        public static Route RegisterResourceRoute(RegisterResourceRouteFactory routeFactory, string resourceRouteName, string resourceFolder, Assembly resourceAssembly, string applicationDefaultNamespace)
        {
            return RegisterResourceRoute(routeFactory, resourceRouteName, "", resourceFolder, resourceAssembly, applicationDefaultNamespace);
        }

        public static Route RegisterResourceRoute(RegisterResourceRouteFactory routeFactory, string resourceRouteName, string resourceBaseUrl, string resourceFolder, Assembly resourceAssembly, string applicationDefaultNamespace)
        {
            Contract.Requires(routeFactory != null);

            var routeHandler = new ResourceRouteHandler(resourceAssembly, $"{applicationDefaultNamespace}.{resourceFolder}");
            var url = GetUrl(resourceBaseUrl, resourceFolder);

            Route route = RegisterRoute(routeFactory, routeHandler, resourceRouteName, url);

            return route;
        }

        private static string GetUrl(params string[] segments)
        {
            return string.Join("/", segments.Union(new[] { $"{{*{RouteConstants.file}}}" }).Where(StringExtensions.IsNotNullOrEmpty));
        }

        private static Route RegisterRoute(RegisterResourceRouteFactory routeFactory, IRouteHandler routeHandler, string resourceRouteName, string url)
        {
            Route route = routeFactory?.Invoke(resourceRouteName, url);
            route.RouteHandler = routeHandler;
            route.RouteExistingFiles = true;

            return route;
        }
    }

}
