using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics.Contracts;

namespace App
{
    using static ResourceRouteHandler;

    public static class RouteDefaults
    {
        public const string Home = nameof(Home);
        public const string Index = nameof(Index);
    }

    public static class RouteConstants
    {
        public const string area = nameof(area);
        public const string controller = nameof(controller);
        public const string action = nameof(action);
        public const string id = nameof(id);
    }


    public abstract class AppAreaRegistration : AreaRegistration
    {
        public const string area_default = nameof(area_default);

        public static class DataTokens
        {
            public const string areaFolder = nameof(areaFolder);
            public const string areaDefaultRouteName = nameof(areaDefaultRouteName);
        }
        public virtual string AreaFolder
        {
            get { return AreaName; }
        }
        public abstract string AreaBaseNamespace
        {
            get;
        }
        public virtual string DefaultUrl => $"{{{RouteConstants.controller}}}/{{{RouteConstants.action}}}/{{{RouteConstants.id}}}";

        public virtual object DefaultUrlValues => new { controller = RouteDefaults.Home, action = RouteDefaults.Index, id = UrlParameter.Optional };

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RegisterDefaultRoutes(context);
        }

        protected Route RegisterDefaultRoutes(AreaRegistrationContext context)
        {
            Contract.Requires(context != null);

            var scriptResourceRouteName = GetAreaResourceRouteName(AreaName, DefaultFolders.Scripts);
            var contentResourceRouteName = GetAreaResourceRouteName(AreaName, DefaultFolders.Content);

            var areaDefaultRouteName = GetAreaDefaultRouteName(AreaName);
            var url = $"{AreaName}/{DefaultUrl}";
            var defaults = DefaultUrlValues;
            var constraints = CreateControllerExcludeConstraintForDefaultFolders();

            Route areaDefaultRoute = context.MapRoute(areaDefaultRouteName, url, defaults, constraints);

            areaDefaultRoute.DataTokens[DataTokens.areaFolder] = AreaFolder;
            areaDefaultRoute.DataTokens[DataTokens.areaDefaultRouteName] = areaDefaultRouteName;

            areaDefaultRoute.DataTokens[ResourceRouteHandler.DataTokens.areaScriptResourceRouteName] = scriptResourceRouteName;
            areaDefaultRoute.DataTokens[ResourceRouteHandler.DataTokens.areaContentResourceRouteName] = contentResourceRouteName;

            RegisterAreaResourceRoute(context, scriptResourceRouteName, DefaultFolders.Scripts);
            RegisterAreaResourceRoute(context, contentResourceRouteName, DefaultFolders.Content);

            return areaDefaultRoute;
        }

        public static string GetAreaDefaultRouteName(string areaName)
        {
            return $"{areaName}_{area_default}";
        }

        protected Route RegisterAreaResourceRoute(AreaRegistrationContext context, string resourceRouteName, string resourceFolder)
        {
            return RegisterResourceRoute(context.MapRoute, resourceRouteName, AreaName, resourceFolder, GetType().Assembly, GetAreaDefaultNamespace());
        }

        private string GetAreaDefaultNamespace()
        {
            return $"{AreaBaseNamespace}.{AreaFolder}";
        }
    }

}
