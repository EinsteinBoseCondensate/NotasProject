using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotasProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            ViewBag.Description = "Bienvenido, tienes cosas que contarnos, escríbelas";
            return View();
        }

        public ActionResult Editor()
        {
            return PartialView("../Notas/CreatePartial");
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Tu espacio alternativo";

            return View();
        }
    }
}