using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using RazorGenerator.Mvc;

namespace App
{

    public class ChangingAreaCompositePrecompiledMvcEngine : CompositePrecompiledMvcEngine
    {
        private class AreaChanger : IDisposable
        {
            private readonly string originalArea;
            private readonly RouteData routeData;

            public AreaChanger(RouteData routeData)
            {
                this.routeData = routeData;
                var areaFolder = routeData.GetAreaFolder();
                originalArea = routeData.GetAreaName();

                routeData.DataTokens[AppAreaRegistration.RouteConstants.area] = areaFolder ?? originalArea;

            }

            public void Dispose()
            {
                routeData.DataTokens[AppAreaRegistration.RouteConstants.area] = originalArea;
            }
        }

        public ChangingAreaCompositePrecompiledMvcEngine(params PrecompiledViewAssembly[] viewAssemblies) : base(viewAssemblies)
        {
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            using (new AreaChanger(controllerContext.RouteData))
                return base.CreateView(controllerContext, viewPath, masterPath);
        }
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            using (new AreaChanger(controllerContext.RouteData))
                return base.CreatePartialView(controllerContext, partialPath);
        }
        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            using (new AreaChanger(controllerContext.RouteData))
                return base.FileExists(controllerContext, virtualPath);
        }
        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            using (new AreaChanger(controllerContext.RouteData))
                return base.FindPartialView(controllerContext, partialViewName, useCache);
        }
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            using (new AreaChanger(controllerContext.RouteData))
                return base.FindView(controllerContext, viewName, masterName, useCache);
        }
        public override void ReleaseView(ControllerContext controllerContext, IView view)
        {
            using (new AreaChanger(controllerContext.RouteData))
                base.ReleaseView(controllerContext, view);
        }
    }
}
