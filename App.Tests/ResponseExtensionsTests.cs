using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using static App.Tests.ChangingAreaCompositePrecompiledMvcEngineTests;

namespace App.Tests
{
    public class ResponseExtensionsTests
    {
        [Theory]
        [InlineData("gif", "image/gif")]
        [InlineData("jpg", "image/jpeg")]
        [InlineData("png", "image/png")]
        [InlineData("js", "text/javascript")]
        [InlineData("css", "text/css")]
        public void Should_set_content_type_based_on_extention(string extension, string contentType)
        {
            var httpContext = new TestHttpContext(new WebTestContext());

            httpContext.Response.SetContentTypeBasedOnExtension(extension);

            Assert.Equal(contentType, httpContext.Response.ContentType);
        }
        [Fact]
        public void Should_throw_exception_from_unknown_extention()
        {
            var httpContext = new TestHttpContext(new WebTestContext());

            var exception = Assert.Throws<NotSupportedException>(() =>
                httpContext.Response.SetContentTypeBasedOnExtension("unknown"));

            Assert.Contains("unknown", exception.Message);

        }
    }
}
