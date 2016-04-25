using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Routing;
using Xunit;

namespace App.Tests
{
    public class ResourceHandlerTests
    {
        private const string TestMemoryStreamMD5 = "C73CABEB6558ABA030BBA9CA49DCDD75";

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

        [Theory]
        [InlineData("missing-file.js", "missing-file.js")]
        [InlineData("", null)]
        [InlineData(null, null)]
        public async Task Should_return_file_not_found_when_file_is_missing(string file, string expectedFile)
        {
            TestHttpContext httpContext;
            TestFileContentProvider testFileContentProvider;
            ResourceHandler sut;

            CreateSut(file, false, out httpContext, out testFileContentProvider, out sut);

            await sut.ProcessRequestAsync();

            Assert.Equal(expectedFile, testFileContentProvider.ExistsRelativePath);

            Assert.Equal(404, httpContext.Response.StatusCode);

            Assert.Equal(0, httpContext.Response.OutputStream.Length);

        }

        [Fact]
        public async Task Should_return_file_when_file_exists()
        {
            TestHttpContext httpContext;
            TestFileContentProvider testFileContentProvider;
            ResourceHandler sut;

            CreateSut("file.js", true, out httpContext, out testFileContentProvider, out sut);

            await sut.ProcessRequestAsync();

            Assert.Equal("file.js", testFileContentProvider.GetContentRelativePath);

            Assert.Equal(200, httpContext.Response.StatusCode);
            Assert.Equal("text/javascript", httpContext.Response.ContentType);


            Assert.Equal(TestMemoryStreamMD5, httpContext.Response.Headers[ResourceHandler.HttpHeaders.ETag]);

            httpContext.Response.OutputStream.Position = 0;

            Assert.Equal(4, httpContext.Response.OutputStream.Length);
            Assert.Equal(4, httpContext.Response.OutputStream.ReadByte());
            Assert.Equal(3, httpContext.Response.OutputStream.ReadByte());
            Assert.Equal(2, httpContext.Response.OutputStream.ReadByte());
            Assert.Equal(1, httpContext.Response.OutputStream.ReadByte());

        }


        [Fact]
        public async Task Should_return_not_modified_when_file_cached_by_client()
        {
            TestHttpContext httpContext;
            TestFileContentProvider testFileContentProvider;
            ResourceHandler sut;

            CreateSut("file.js", true, out httpContext, out testFileContentProvider, out sut);

            httpContext.Request.Headers.Add(ResourceHandler.HttpHeaders.If_None_Match, TestMemoryStreamMD5);

            await sut.ProcessRequestAsync();

            Assert.Equal("file.js", testFileContentProvider.GetContentRelativePath);

            Assert.Equal(304, httpContext.Response.StatusCode);


        }

        private static void CreateSut(string file, bool fileExists, out TestHttpContext httpContext, out TestFileContentProvider testFileContentProvider, out ResourceHandler handler)
        {
            httpContext = new TestHttpContext(new WebTestContext { AppRelativeCurrentExecutionFilePath = $"~/Scripts/" + file });
            var route = new Route("Scripts/{*file}", null)
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
