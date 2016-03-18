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
    public abstract class AppAreaRegistration : AreaRegistration
    {
        private const string Content = nameof(Content);
        private const string Scripts = nameof(Scripts);

        public static class RouteConstants
        {
            public const string areaFolder = nameof(areaFolder);
            public const string area = nameof(area);
            public const string controller = nameof(controller);
            public const string action = nameof(action);
            public const string id = nameof(id);
        }

        public virtual string AreaFolder
        {
            get { return AreaName; }
        }
        public abstract string BaseAreaNamespace
        {
            get;
        }
        public virtual string DefaultUrl => $"{{{RouteConstants.controller}}}/{{{RouteConstants.action}}}/{{{RouteConstants.id}}}";

        public virtual object DefaultUrlValues => new { controller = "Home", action = "Index", id = UrlParameter.Optional };

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RegisterResourceRoute(context, Scripts);
            RegisterResourceRoute(context, Content);

            RegisterDefaultRoute(context);
        }

        private Route RegisterDefaultRoute(AreaRegistrationContext context)
        {
            Route route = context.MapRoute(
                $"{AreaName}_default",
                $"{AreaName}/{DefaultUrl}",
                DefaultUrlValues,
                namespaces: new[] { $"{BaseAreaNamespace}.{AreaFolder}.Controllers" }
            );
            route.DataTokens.Add(RouteConstants.areaFolder, AreaFolder);
            return route;
        }

        protected Route RegisterResourceRoute(AreaRegistrationContext context, string resourcePrefix)
        {
            Contract.Requires(!string.IsNullOrEmpty(AreaName), "AreaName is null or empty.");
            Contract.Requires(!string.IsNullOrEmpty(BaseAreaNamespace), "AreaName is null or empty.");
            Contract.Requires(!string.IsNullOrEmpty(AreaFolder), "AreaName is null or empty.");
            Contract.Requires(context != null, "context is null.");
            Contract.Requires(!string.IsNullOrEmpty(resourcePrefix), "resourcePrefix is null or empty.");

            return ResourceRouteHandler.RegisterResourceRoute(context.MapRoute, AreaName, resourcePrefix, GetType().Assembly, $"{BaseAreaNamespace}.{AreaFolder}.{resourcePrefix}");
        }
    }

}
