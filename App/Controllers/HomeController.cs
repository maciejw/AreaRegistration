using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexHome2()
        {
            return PartialView("../Home2/Index");
        }
        public ActionResult IndexSub()
        {
            return View("Sub/Index");
        }
    }
}
