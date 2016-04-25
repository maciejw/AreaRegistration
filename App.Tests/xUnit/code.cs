using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace App.Tests
{
    [ExcludeFromCodeCoverage]
    public class CategoryDiscoverer : ITraitDiscoverer
    {
        public const string Key = "Category";

        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            var ctorArgs = traitAttribute.GetConstructorArguments().ToList();
            yield return new KeyValuePair<string, string>(Key, ctorArgs[0].ToString());
        }
    }

    [TraitDiscoverer("App.Tests.CategoryDiscoverer", "App.Tests")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [ExcludeFromCodeCoverage]
    public class CategoryAttribute : Attribute, ITraitAttribute
    {
        public CategoryAttribute(string category) { }
    }

    [ExcludeFromCodeCoverage]
    public class PerformanceTraitDiscoverer : ITraitDiscoverer
    {
        public const string Value = "Performance";

        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            yield return new KeyValuePair<string, string>(CategoryDiscoverer.Key, Value);
        }
    }

    [TraitDiscoverer("App.Tests.PerformanceTraitDiscoverer", "App.Tests")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [ExcludeFromCodeCoverage]
    public class PerformanceTraitAttribute : Attribute, ITraitAttribute
    {
        public PerformanceTraitAttribute()
        {
        }
    }
}
