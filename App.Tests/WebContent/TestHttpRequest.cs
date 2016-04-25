using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace App.Tests
{




    public class TestHttpRequest : HttpRequestBase
    {
        private readonly WebTestContext context;
        private readonly HttpCookieCollection httpCookieCollection;
        private readonly TestHttpBrowserCapabilitiesBase testHttpBrowserCapabilitiesBase;
        private readonly NameValueCollection httpHeadersCollection;

        public TestHttpRequest(WebTestContext context)
        {
            this.context = context;
            testHttpBrowserCapabilitiesBase = new TestHttpBrowserCapabilitiesBase(context);
            httpCookieCollection = new HttpCookieCollection();
            httpHeadersCollection = new NameValueCollection();

        }
        public override HttpCookieCollection Cookies => httpCookieCollection;
        public override HttpBrowserCapabilitiesBase Browser => testHttpBrowserCapabilitiesBase;

        public override string AppRelativeCurrentExecutionFilePath => context.AppRelativeCurrentExecutionFilePath;
        public override string PathInfo => "";
        public override string UserAgent => "TestAgent";
        public override bool IsLocal => true;
        public override string ApplicationPath => "";
        public override NameValueCollection Headers => httpHeadersCollection;
    }

}
