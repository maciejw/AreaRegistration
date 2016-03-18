using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using RazorGenerator.Mvc;
using System;
using System.Linq;
using System.Web.Routing;
using System.Diagnostics.Contracts;
using System.Reflection;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(App.RazorGeneratorMvcStart), "Start")]

namespace App
{
    public static class RazorGeneratorMvcStart
    {
        public static void Start()
        {
            ChangingAreaCompositePrecompiledMvcEngine engine = CreateViewEngine();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(engine);

            VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
        }

        private static ChangingAreaCompositePrecompiledMvcEngine CreateViewEngine()
        {
            var thisAssembly = typeof(RazorGeneratorMvcStart).Assembly.GetName();

            Func<AssemblyName, bool> ofThisAssembly = an =>
                an.FullName == thisAssembly.FullName;

            Func<Assembly, bool> assemblyReferencesThisAssembly = a =>
                a.GetReferencedAssemblies().Any(ofThisAssembly);

            Func<Assembly, PrecompiledViewAssembly> precompiledViewAssembly = a =>
                new PrecompiledViewAssembly(a)
                {
                    UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal
                };

            var modules = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(assemblyReferencesThisAssembly)
                .Select(precompiledViewAssembly).ToArray();

            return new ChangingAreaCompositePrecompiledMvcEngine(modules);
        }
    }
}
