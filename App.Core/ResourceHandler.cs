using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Diagnostics.Contracts;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace App
{
    public class ResourceHandler : HttpHandlerBase
    {
        private readonly IFileContentProvider fileContentProvider;

        public ResourceHandler(RequestContext requestContext, IFileContentProvider fileContentProvider)
            : base(requestContext)
        {
            this.fileContentProvider = fileContentProvider;
        }

        public override void ProcessRequest()
        {
            object path;
            if (routeData.Values.TryGetValue(ResourceRouteHandler.RouteConstants.path, out path) && path != null)
            {
                var relativePath = path as string;
                if (!fileContentProvider.Exists(relativePath))
                {
                    context.Response.StatusCode = 404;
                    return;
                }

                var stream = fileContentProvider.GetContent(relativePath);

                context.Response.SetContentTypeBasedOnExtension(Path.GetExtension(relativePath).Replace(".", ""));

                context.Response.StatusCode = 200;

                stream.CopyTo(context.Response.OutputStream);
            }
        }
    }
}
