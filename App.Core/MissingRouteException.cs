using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace App
{
    [ExcludeFromCodeCoverage]
    public class MissingResourceRouteException : Exception
    {
        public MissingResourceRouteException(string routeName)
            : base($"Missing route. {routeName} not registered")
        {
        }
    }
}
