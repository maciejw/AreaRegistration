using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Areas.CustomArea1.Controllers
{
    public class HomeController : Controller
    {
        // GET: CustomArea1/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}
