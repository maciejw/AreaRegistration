using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace App
{
    [ExcludeFromCodeCoverage]
    public class Exceptions
    {
        public static Exception UnsupportedExtenion<T>(T extension)
        {
            return new NotSupportedException($"Extension {extension} not supported");
        }

        public static Exception MissingRouteException(string route)
        {
            return new MissingResourceRouteException(route);
        }
    }
}
