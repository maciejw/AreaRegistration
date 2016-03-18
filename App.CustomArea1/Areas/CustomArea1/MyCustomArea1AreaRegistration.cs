using System;
using System.Linq;
using System.Web.Mvc;

namespace App.Areas.CustomArea1
{
    public class MyCustomArea1AreaRegistration : CustomArea1AreaRegistration
    {
        private const string MyCustomArea1 = nameof(MyCustomArea1);

        public override string AreaName => MyCustomArea1;
    }
}
