using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WDPlatform.Controllers
{
    public class HomeController : Controller
    {
        // GET: TestChat
        public ActionResult TestChat()
        {
            return View();
        }

        // GET: Index
        public ActionResult Index()
        {
            return View();
        }


        // GET: Join room
        public ActionResult Game(int number)
        {
            return View(number);
        }
    }
}