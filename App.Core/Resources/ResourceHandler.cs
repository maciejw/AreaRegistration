using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Diagnostics.Contracts;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace App
{
    public class ResourceHandler : HttpHandlerBase
    {
        public string RelativePath { get; set; }
        public Stream Content { get; set; }
        public string ETagValue { get; set; }

        public static class HttpHeaders
        {
            public static readonly string If_None_Match = nameof(If_None_Match).ReplaceUnderscoresWithHyphens();
            public static readonly string ETag = nameof(ETag);
        }

        public static ConcurrentDictionary<string, string> fileEtagCache = new ConcurrentDictionary<string, string>();

        private static readonly MD5 md5 = MD5.Create();

        private readonly IFileContentProvider fileContentProvider;

        public ResourceHandler(RequestContext requestContext, IFileContentProvider fileContentProvider)
            : base(requestContext)
        {
            this.fileContentProvider = fileContentProvider;
        }

        public async override Task ProcessRequestAsync()
        {
            var pipelineSteps = new Func<Func<Task>, Task>[]
            {
                HandleInput,
                HandleFileNotFound,
                HandleRetrieveContent,
                HandleNotModified
            };

            var handle = pipelineSteps.Aggregate(Compose);

            await handle(() => ReturnResource());
        }

        public static Func<Func<Task>, Task> Compose(Func<Func<Task>, Task> f1, Func<Func<Task>, Task> f2) => f => f1(() => f2(f));

        private Task HandleInput(Func<Task> next)
        {
            object file;

            if (routeData.Values.TryGetValue(ResourceRouteHandler.RouteConstants.file, out file) && file != null)
            {
                RelativePath = file as string;
            }

            return next();
        }


        private Task HandleFileNotFound(Func<Task> next)
        {
            var fileNotFound = !fileContentProvider.Exists(RelativePath);

            if (fileNotFound)
            {
                context.Response.StatusCode = 404;
                return Task.CompletedTask;
            }
            return next();
        }

        private Task HandleRetrieveContent(Func<Task> next)
        {
            Content = fileContentProvider.GetContent(RelativePath);

            ETagValue = GetETagValue(Content);

            return next();
        }

        private Task HandleNotModified(Func<Task> next)
        {
            var cachedFileValid = ETagValue.Equals(context.Request.Headers[HttpHeaders.If_None_Match], StringComparison.CurrentCultureIgnoreCase);

            if (cachedFileValid)
            {
                context.Response.StatusCode = 304;
                return Task.CompletedTask;
            }

            return next();
        }


        private Task ReturnResource()
        {
            context.Response.StatusCode = 200;
            context.Response.SetContentTypeBasedOnExtension(Path.GetExtension(RelativePath).Replace(".", ""));
            context.Response.AppendHeader(HttpHeaders.ETag, ETagValue);

            return Content.CopyToAsync(context.Response.OutputStream);
        }

        private string GetETagValue(Stream stream)
        {
            return fileEtagCache.GetOrAdd(context.Request.AppRelativeCurrentExecutionFilePath, s =>
            {
                var hash = md5.ComputeHash(stream);
                stream.Position = 0;
                return BitConverter.ToString(hash).Replace("-", "");
            });
        }


    }


}
