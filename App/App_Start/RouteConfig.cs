using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App
{
    public class RouteConfig
    {
        private const string Content = nameof(Content);
        private const string Scripts = nameof(Scripts);

        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.RouteExistingFiles = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            ResourceRouteHandler.RegisterResourceRoute(routes.MapRoute, Content, typeof(RouteConfig).Assembly, $"{nameof(App)}.{Content}");
            ResourceRouteHandler.RegisterResourceRoute(routes.MapRoute, Scripts, typeof(RouteConfig).Assembly, $"{nameof(App)}.{Scripts}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "App.Controllers" }
            );
        }
    }
}

