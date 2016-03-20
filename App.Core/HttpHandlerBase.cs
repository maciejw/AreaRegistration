using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Diagnostics.Contracts;

namespace App
{
    [ExcludeFromCodeCoverage]
    public abstract class HttpHandlerBase : IHttpHandler
    {
        protected readonly HttpContextBase context;
        protected readonly RouteData routeData;

        public HttpHandlerBase(RequestContext requestContext)
        {
            Contract.Requires(requestContext != null);

            this.routeData = requestContext.RouteData;
            this.context = requestContext.HttpContext;
        }

        bool IHttpHandler.IsReusable { get { return false; } }

        void IHttpHandler.ProcessRequest(HttpContext c)
        {
            ProcessRequest();
        }

        public abstract void ProcessRequest();
    }
}
