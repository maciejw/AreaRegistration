using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace App
{
    [ExcludeFromCodeCoverage]
    public abstract class HttpHandlerBase : HttpTaskAsyncHandler
    {
        protected readonly HttpContextBase context;
        protected readonly RouteData routeData;

        public HttpHandlerBase(RequestContext requestContext)
        {
            Contract.Requires(requestContext != null);

            this.routeData = requestContext.RouteData;
            this.context = requestContext.HttpContext;
        }

        public sealed override Task ProcessRequestAsync(HttpContext context)
        {
            return ProcessRequestAsync();
        }
        public sealed override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
        }

        public abstract Task ProcessRequestAsync();
    }
}
