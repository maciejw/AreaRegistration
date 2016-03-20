using System;
using System.Linq;
using System.Web.Mvc;

namespace App.Areas.CustomArea1
{
    public class CustomArea1AreaRegistration : AppAreaRegistration
    {
        public override string AreaName => nameof(CustomArea1);

        public override string AreaBaseNamespace => $"{nameof(App)}.{nameof(App.Areas)}";

        public override string AreaFolder => nameof(CustomArea1);

    }
}
