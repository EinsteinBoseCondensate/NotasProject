using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotasProject.Controllers
{
    public class AjaxModalsController : Controller
    {
        public ActionResult ModalExitoPartial(string msg)
        {
            ViewBag.ModalExitoMessage = msg??"La operación se realizó correctamente";
            return PartialView();
        }
        public ActionResult ModalErrorPartial(string msg)
        {
            //msg = msg.Replace("$$$", " ");
            ViewBag.ModalErrorMessage = msg??"La operación no ha podido llevarse a cabo, disculpa las molestias";
            return PartialView();
        }
    }
}