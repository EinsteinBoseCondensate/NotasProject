using NotasProject.Models;
using NotasProject.Models.Config;
using NotasProject.Models.DTOs;
using NotasProject.Models.Jsons;
using NotasProject.Repositories;
using NotasProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using NotasProject.Properties;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace NotasProject.Controllers
{
    [Authorize]
    public class NotasController : BaseController
    {
        private NotasService _notasService;
        public NotasController(NotasService notasService) :base()
        {
            _notasService = notasService;
        }

        public ActionResult Index()
        {
            ViewBag.Title = Resources.NotasIndexTitle;
            ViewBag.Description = Resources.NotasIndexMessage;
            return View();
        }

        [ValidateAntiForgeryToken, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Create(CreateNotaDTO dto)
        {
            var tuple = await Task.Run(() => _notasService.Create(dto, GetCurrentUser()) );
            if (tuple.Item1 == PersistedState.OK && ExistsKey(Resources.MisNotas))
            {
                NotasCache cached = GetSessionItem<NotasCache>(Resources.MisNotas);
                if (dto.Anchor)
                {
                    cached.anchored.Add(tuple.Item2);
                    cached.anchored = cached.anchored.OrderByDescending(nota => nota.CDT).ToList();
                }
                else
                {
                    cached.notAnchored.Add(tuple.Item2);
                    cached.notAnchored = cached.notAnchored.OrderByDescending(nota => nota.CDT).ToList();
                }
                RemoveFromSession(Resources.MisNotas);//Como aprovecho parte de la key para saber el nonce del AES y este nonce se genera automáticamente y siempre es distinto, no me lo va a sobreescribir en caché, me va a crear un NotasCache cada vez!!!
                SetSessionItem(Resources.MisNotas, cached);
            }
            return SimpleJSONFeedback(tuple.Item1);
        }

        public ActionResult Editor(string option)
        {
            if(option == Resources.NotasEditorParamOptionEdit)
            {
                ViewBag.Edit = true;
            }
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Save(NotaDTO dto)
        {
            PersistedState ps = _notasService.Edit(dto);
            if(ps == PersistedState.OK)
            {
                NotasCache cached = GetSessionItem<NotasCache>(Resources.MisNotas);
                RemoveFromSession(Resources.MisNotas);
                SetSessionItem(Resources.MisNotas, _notasService.UpdateCache(cached, dto));
            }
            return SimpleJSONFeedback(ps);
        }
        //[ValidateAntiForgeryToken]
        public ActionResult MisNotas()
        {
            NotasCache response;
            if (ExistsKey(Resources.MisNotas)) {
                response = GetSessionItem<NotasCache>(Resources.MisNotas);
            } else {
                response = _notasService.GetNotasByUserId(GetCurrentUser().Id);
                SetSessionItem(Resources.MisNotas,response);
                    }
            return PartialView(response);
        }
        [HttpPost]
        public ActionResult Delete(SinglePropJson json)
        {
            PersistedState ps = _notasService.Remove(int.Parse(json.Value));
            if (ps == PersistedState.OK)
            {
                NotasCache cached = GetSessionItem<NotasCache>(Resources.MisNotas);
                RemoveFromSession(Resources.MisNotas);
                SetSessionItem(Resources.MisNotas, _notasService.RemoveFromCache(cached, json));
            }

            return SimpleJSONFeedback(ps);
        }
    }
} 
