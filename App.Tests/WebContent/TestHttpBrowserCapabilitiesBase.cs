using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Tests
{
    

    

    

    public class TestHttpBrowserCapabilitiesBase : HttpBrowserCapabilitiesBase
    {
        private readonly WebTestContext context;

        public TestHttpBrowserCapabilitiesBase(WebTestContext context)
        {
            this.context = context;
        }

        public override bool IsMobileDevice => false;
    }

}
