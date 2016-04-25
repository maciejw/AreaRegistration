using System;
using System.Linq;
using System.Web.Mvc;
using static App.ResourceRouteHandler.DataTokens;

namespace App
{
    public static class UrlHelperExtensions
    {
        private static string ResourceRouteUrl(UrlHelper @this, string routeKey, string file)
        {
            string routeName = @this.RequestContext.RouteData.DataTokens[routeKey] as string;

            if (routeName.IsNullOrEmpty())
            {
                throw new MissingResourceRouteException(routeKey);
            }

            return @this.RouteUrl(routeName, new { file });
        }

        public static string AreaScript(this UrlHelper @this, string scriptPath)
        {
            return ResourceRouteUrl(@this, areaScriptResourceRouteName, scriptPath);
        }

        public static string AreaContent(this UrlHelper @this, string contentPath)
        {
            return ResourceRouteUrl(@this, areaContentResourceRouteName, contentPath);
        }

        public static string DefaultScript(this UrlHelper @this, string scriptPath)
        {
            return ResourceRouteUrl(@this, defaultScriptResourceRouteName, scriptPath);
        }

        public static string DefaultContent(this UrlHelper @this, string contentPath)
        {
            return ResourceRouteUrl(@this, defaultContentResourceRouteName, contentPath);
        }

    }
}
