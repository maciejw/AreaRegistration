using System;
using System.Linq;
using System.Web.Mvc;

namespace App.Areas.CustomArea1
{
    public class AnotherCustomArea1AreaRegistration : CustomArea1AreaRegistration
    {
        private const string AnotherCustomArea1 = nameof(AnotherCustomArea1);

        public override string AreaName => AnotherCustomArea1;
    }
}
