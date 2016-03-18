using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Diagnostics.Contracts;

namespace App
{


    public class ResourceHandler : IHttpHandler
    {
        readonly private IFileContentProvider fileContentProvider;
        readonly private RequestContext requestContext;

        public ResourceHandler(RequestContext requestContext, IFileContentProvider fileContentProvider)
        {
            this.fileContentProvider = fileContentProvider;
            this.requestContext = requestContext;

        }

        public bool IsReusable => false;

        public void ProcessRequest(HttpContext c)
        {

            HttpContextBase context = new HttpContextWrapper(c);

            object path;
            if (requestContext.RouteData.Values.TryGetValue(ResourceRouteHandler.RouteConstants.path, out path) && path != null)
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

    public enum ContentExtensions
    {
        css,
        js,
        png,
        jpg,
        gif
    }
    public class MediaTypeNames
    {
        public const string Css = "text/css";
        public const string Js = "text/javascript";
        public const string Png = "image/png";
        public const string Jpeg = System.Net.Mime.MediaTypeNames.Image.Jpeg;
        public const string Gif = System.Net.Mime.MediaTypeNames.Image.Gif;
    }
    public static class HttpResponseExtensions
    {

        public static void SetContentTypeBasedOnExtension(this HttpResponseBase @this, string extension)
        {
            Contract.Requires(@this != null, "this is null.");
            Contract.Requires(!string.IsNullOrEmpty(extension), "extension is null or empty.");

            ContentExtensions ext;
            if (!Enum.TryParse(extension, out ext))
            {
                throw Exceptions.UnsupportedExtenion(extension);
            }

            switch (ext)
            {
                case ContentExtensions.css:
                    @this.ContentType = MediaTypeNames.Css;
                    break;
                case ContentExtensions.js:
                    @this.ContentType = MediaTypeNames.Js;
                    break;
                case ContentExtensions.png:
                    @this.ContentType = MediaTypeNames.Png;
                    break;
                case ContentExtensions.jpg:
                    @this.ContentType = MediaTypeNames.Jpeg;
                    break;
                case ContentExtensions.gif:
                    @this.ContentType = MediaTypeNames.Gif;
                    break;
                default:
                    throw Exceptions.UnsupportedExtenion(extension);

            }
        }
    }

    public class Exceptions
    {
        public static Exception UnsupportedExtenion<T>(T extension)
        {
            return new NotSupportedException($"Extension {extension} not supported");
        }
    }
}
