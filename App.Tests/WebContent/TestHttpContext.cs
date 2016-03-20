using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Tests
{
    

    public class TestHttpContext : HttpContextBase
    {
        private readonly WebTestContext context;
        private readonly TestHttpRequest testHttpRequest;
        private readonly TestHttpResponse testHttpResponse;
        private readonly Hashtable hashtable;


        public TestHttpContext(WebTestContext context)
        {
            this.context = context;
            testHttpRequest = new TestHttpRequest(context);
            testHttpResponse = new TestHttpResponse(context);
            hashtable = new Hashtable();
        }
        public override IDictionary Items => hashtable;
        public override HttpRequestBase Request => testHttpRequest;
        public override HttpResponseBase Response => testHttpResponse;


        public override object GetService(Type serviceType)
        {
            return null;
        }
    }

}
