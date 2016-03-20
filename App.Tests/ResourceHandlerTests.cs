using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using Xunit;

namespace App.Tests
{
    public class ResourceHandlerTests
    {
        public class TestFileContentProvider : IFileContentProvider
        {
            private readonly MemoryStream memoryStream;
            private readonly bool fileExists;

            public string GetContentRelativePath { get; private set; }
            public string ExistsRelativePath { get; private set; }

            public TestFileContentProvider(bool fileExists)
            {
                this.fileExists = fileExists;
                this.memoryStream = CreateTestStream();
            }

            public bool Exists(string relativePath)
            {
                this.ExistsRelativePath = relativePath;

                return fileExists;
            }

            public Stream GetContent(string relativePath)
            {
                this.GetContentRelativePath = relativePath;
                return memoryStream;
            }

            private static MemoryStream CreateTestStream()
            {
                MemoryStream memoryStream = new MemoryStream();

                memoryStream.WriteByte(4);
                memoryStream.WriteByte(3);
                memoryStream.WriteByte(2);
                memoryStream.WriteByte(1);

                memoryStream.Position = 0;

                return memoryStream;
            }

        }

        [Fact]
        public void Should_not_return_file_when_file_exists()
        {
            TestHttpContext httpContext;
            TestFileContentProvider testFileContentProvider;
            ResourceHandler handler;

            CreateSut("missing-file.js", false, out httpContext, out testFileContentProvider, out handler);

            handler.ProcessRequest();

            Assert.Equal("missing-file.js", testFileContentProvider.ExistsRelativePath);

            Assert.Equal(404, httpContext.Response.StatusCode);

            Assert.Equal(0, httpContext.Response.OutputStream.Length);

        }

        [Fact]
        public void Should_return_file_when_file_exists()
        {
            TestHttpContext httpContext;
            TestFileContentProvider testFileContentProvider;
            ResourceHandler handler;

            CreateSut("file.js", true, out httpContext, out testFileContentProvider, out handler);

            handler.ProcessRequest();

            Assert.Equal("file.js", testFileContentProvider.GetContentRelativePath);

            Assert.Equal(200, httpContext.Response.StatusCode);
            Assert.Equal("text/javascript", httpContext.Response.ContentType);

            httpContext.Response.OutputStream.Position = 0;

            Assert.Equal(4, httpContext.Response.OutputStream.Length);
            Assert.Equal(4, httpContext.Response.OutputStream.ReadByte());
            Assert.Equal(3, httpContext.Response.OutputStream.ReadByte());
            Assert.Equal(2, httpContext.Response.OutputStream.ReadByte());
            Assert.Equal(1, httpContext.Response.OutputStream.ReadByte());

        }

        private static void CreateSut(string file, bool fileExists, out TestHttpContext httpContext, out TestFileContentProvider testFileContentProvider, out ResourceHandler handler)
        {
            httpContext = new TestHttpContext(new WebTestContext { AppRelativeCurrentExecutionFilePath = $"~/Scripts/" + file });
            var route = new Route("Scripts/{*path}", null)
            {
                Defaults = new RouteValueDictionary(),
                DataTokens = new RouteValueDictionary()
            };


            var requestContext = new RequestContext(httpContext, route.GetRouteData(httpContext));

            testFileContentProvider = new TestFileContentProvider(fileExists);
            handler = new ResourceHandler(requestContext, testFileContentProvider);
        }
    }
}
