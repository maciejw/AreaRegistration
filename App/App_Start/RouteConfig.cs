using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var defaultNamespace = nameof(App);

            var url = $"{{{RouteConstants.controller}}}/{{{RouteConstants.action}}}/{{{RouteConstants.id}}}";
            var defaults = new { controller = RouteDefaults.Home, action = RouteDefaults.Index, id = UrlParameter.Optional };
            var namespaces = new[] { $"{defaultNamespace}.Controllers" };
            var currentAssembly = Assembly.GetCallingAssembly();

            routes.RegisterDefaultRoutes(url, defaults, namespaces, currentAssembly, defaultNamespace);

        }
    }
}

