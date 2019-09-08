using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotasProject.Properties;

namespace NotasProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = Resources.HomeIndexTitle;
            ViewBag.Description = Resources.HomeIndexDescription;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Title = Resources.HomeContactTitle;
            ViewBag.Description = Resources.HomeContactDescription;

            return View();
        }
    }
}