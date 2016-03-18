using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace App
{
    public static class AreaHelpers
    {
        public static string GetAreaName(this RouteBase route)
        {
            IRouteWithArea routeWithArea = route as IRouteWithArea;
            if (routeWithArea != null)
            {
                return routeWithArea.Area;
            }

            Route castRoute = route as Route;
            if (castRoute != null && castRoute.DataTokens != null)
            {
                return castRoute.DataTokens[AppAreaRegistration.RouteConstants.area] as string;
            }

            return null;
        }

        public static string GetAreaName(this RouteData routeData)
        {
            Contract.Requires(routeData != null, "routeData is null.");
            object area;
            if (routeData.DataTokens.TryGetValue(AppAreaRegistration.RouteConstants.area, out area))
            {
                return area as string;
            }

            return routeData.Route.GetAreaName();
        }

        public static string GetAreaFolder(this RouteData routeData)
        {
            Contract.Requires(routeData != null, "routeData is null.");

            object areaFolder;
            if (routeData.DataTokens.TryGetValue(AppAreaRegistration.RouteConstants.areaFolder, out areaFolder))
            {
                return areaFolder as string;
            }

            return null;
        }
    }
}
