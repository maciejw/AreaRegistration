using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Tests
{
    public class TestHttpResponse : HttpResponseBase
    {
        private readonly WebTestContext context;
        private readonly HttpCookieCollection httpCookieCollection;
        private readonly Stream outputStream;

        public TestHttpResponse(WebTestContext context)
        {
            this.context = context;
            httpCookieCollection = new HttpCookieCollection();
            outputStream = new MemoryStream();
        }

        public override HttpCookieCollection Cookies => httpCookieCollection;
        public override string ContentType { get; set; }
        public override int StatusCode { get; set; }
        public override Stream OutputStream { get { return outputStream; } }

        public override string ApplyAppPathModifier(string virtualPath)
        {
            return virtualPath;
        }
    }

}
