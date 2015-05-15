using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(string msg)
        {
            ViewBag.msg = msg; 
            return View();
        }
    }
}