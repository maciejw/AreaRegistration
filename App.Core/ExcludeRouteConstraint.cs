using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace App
{
    public class ExcludeRouteConstraint : IRouteConstraint
    {
        private readonly string[] excludes;

        public ExcludeRouteConstraint(params string[] excludes)
        {
            this.excludes = excludes;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string value = values[parameterName.ToLower()] as string;

            return !this.excludes.Any(s => s.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return $"exclude [{string.Join(", ", excludes)}]";
        }
    }

}
