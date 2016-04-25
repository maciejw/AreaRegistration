using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        private readonly NameValueCollection httpHeadersCollection;

        public TestHttpResponse(WebTestContext context)
        {
            this.context = context;
            httpCookieCollection = new HttpCookieCollection();
            outputStream = new MemoryStream();
            httpHeadersCollection = new NameValueCollection();

        }

        public override HttpCookieCollection Cookies => httpCookieCollection;
        public override string ContentType { get; set; }
        public override int StatusCode { get; set; }
        public override Stream OutputStream { get { return outputStream; } }
        public override NameValueCollection Headers => httpHeadersCollection;

        public override void AppendHeader(string name, string value)
        {
            Headers.Add(name, value);
        }


        public override string ApplyAppPathModifier(string virtualPath) => virtualPath;
    }

}
