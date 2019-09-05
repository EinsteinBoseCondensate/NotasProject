using NotasProject.Models;
using NotasProject.Models.Config;
using NotasProject.Models.DTOs;
using NotasProject.Models.Jsons;
using NotasProject.Repositories;
using NotasProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NotasProject.Controllers
{
    [Authorize]
    public class NotasController : BaseController
    {
        private NotasService _notasService;
        public NotasController(NotasService notasService, UserService userServ) :base(userServ)
        {
            _notasService = notasService;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Notas";
            ViewBag.Description = "Tu espacio para crear notas, consultarlas y editarlas";
            return View();
        }

        [ValidateAntiForgeryToken, HttpPost, ValidateInput(false)]
        public ActionResult Create(CreateNotaDTO dto)
        {
            return SimpleJSONFeedback(_notasService.Create(dto, GetCurrentUser()));
        }

        public ActionResult Editor(string option)
        {
            if(option == "Edit")
            {
                ViewBag.Edit = true;
            }
            return PartialView("CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Save(NotaDTO dto)
        {
            return SimpleJSONFeedback(_notasService.Edit(dto));
        }
        //[ValidateAntiForgeryToken]
        public ActionResult MisNotas()
        {
            var response = _notasService.GetNotasByUserId(GetCurrentUser().Id);
            return PartialView(response);
        }
        [HttpPost]
        public ActionResult Delete(SinglePropJson json)
        {
            return SimpleJSONFeedback(_notasService.Remove(int.Parse(json.Value)));
        }
    }
}
