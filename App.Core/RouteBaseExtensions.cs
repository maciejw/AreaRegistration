using System;


using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;

namespace App
{
    public static class RouteExtensions
    {
        public static string GetAreaName(this RouteBase route)
        {
            Route castRoute = route as Route;
            return castRoute?.DataTokens?[RouteConstants.area] as string;
        }

        public static bool IsAreaDefaultRoute(this Route route)
        {
            return (route.GetAreaRouteName()?.EndsWith(AppAreaRegistration.area_default)).GetValueOrDefault();
        }
        public static string GetAreaRouteName(this Route route)
        {
            return route?.DataTokens?[AppAreaRegistration.DataTokens.areaDefaultRouteName] as string;
        }

        public static string GetAreaName(this RouteData routeData)
        {
            return routeData?.Route.GetAreaName();
        }

        public static string GetAreaFolder(this RouteData routeData)
        {
            return routeData?.DataTokens?[AppAreaRegistration.DataTokens.areaFolder] as string;

        }
    }
}
