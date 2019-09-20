using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotasProject.Properties;

namespace NotasProject.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var checker = ExistsKey(Resources.ConfirmEmailOKFlag);
            ViewBag.Title = Resources.HomeIndexTitle;
            ViewBag.Description = Request.IsAuthenticated ?
                                        checker ? Resources.HomeIndexDescriptionOnFirstAuth : Resources.HomeIndexDescriptionOnAuth 
                                  : Resources.HomeIndexDescriptionUnAuth;
            if (checker)
            {
                RemoveFromSession(Resources.ConfirmEmailOKFlag);
            }
            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Title = Resources.HomeContactTitle;
            ViewBag.Description = Resources.HomeContactDescription;

            return View();
        }
    }
}