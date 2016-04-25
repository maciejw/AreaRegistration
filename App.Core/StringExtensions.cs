using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public static class StringExtensions
    {
        public static string ReplaceUnderscoresWithHyphens(this string @this)
        {
            return (@this + "").Replace("_", "-");

        }

        public static bool IsNullOrEmpty(this string @this) => string.IsNullOrEmpty(@this);
        public static bool IsNotNullOrEmpty(this string @this) => !string.IsNullOrEmpty(@this);
    }

}
