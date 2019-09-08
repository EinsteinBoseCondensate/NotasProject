using NotasProject.Models;
using NotasProject.Models.Config;
using NotasProject.Models.DTOs;
using NotasProject.Models.Jsons;
using NotasProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NotasProject.Services
{
    public class NotasService
    {
        private NotasRepository _notasRepo; 
        public NotasService(NotasRepository notasRepo)
        {
            _notasRepo = notasRepo;
        }
        public Nota FindById(int id)
        {
            return _notasRepo.BuildQuery().FirstOrDefault(n => n.NotaId == id);
        }
        public Tuple<PersistedState, NotaDTO> Create(CreateNotaDTO dto, ApplicationUser user)
        {
            Nota nota = Mappings.ConvertToNota(dto, user);
            return Tuple.Create(_notasRepo.CreateNote(nota), Mappings.ConvertFromNota(nota));
        }
        public PersistedState Edit(NotaDTO dto)
        {
            Nota nota = FindById(dto.NotaId);
            AES_Result res = CryptoService.AES_Encrypt(dto.NoteContent);
            nota.NoteContent = res.CipherData;
            nota.Nonce = res.Nonce;
            nota.LUDT = DateTime.Now;
            PersistedState ps = _notasRepo.TrySaveChanges();
            _notasRepo.Dispose();
            return ps;
        }
        public NotasCache GetNotasByUserId(string userId)
        {
            List<NotaDTO> notaDtoList = new List<NotaDTO>();
            _notasRepo.BuildQuery().Where(x => x.User.Id == userId).ToList().ForEach(note => notaDtoList.Add(Mappings.ConvertFromNota(note)));
            _notasRepo.Dispose();
            return new NotasCache() {  anchored = notaDtoList.Where(note => note.Anchor).OrderByDescending(x => x.CDT).ToList(), notAnchored = notaDtoList.Where(note => !note.Anchor).OrderByDescending(x => x.CDT).ToList()};
        }

        public PersistedState Remove(int id)
        {
            return _notasRepo.RemoveNotaById(id);
        }
        public NotasCache UpdateCache(NotasCache cached, NotaDTO updated)
        {
            updated.LUDT = DateTime.Now;
            if (updated.Anchor)
            {
                if (cached.anchored.Any(nota => nota.NotaId == updated.NotaId))//No habría cambiado la prioridad
                {
                    NotaDTO mod = cached.anchored.First(nota => nota.NotaId == updated.NotaId);
                    mod.NoteContent = updated.NoteContent;
                }
                else//eliminar y acoplar en el otro grupo
                {
                    cached.anchored.Add(updated);
                    cached.anchored = cached.anchored.OrderByDescending(nota => nota.CDT).ToList();
                    cached.notAnchored.Remove(cached.notAnchored.First(nota => nota.NotaId == updated.NotaId));
                }
            }
            else
            {
                if (cached.notAnchored.Any(nota => nota.NotaId == updated.NotaId))//No habría cambiado la prioridad
                {
                    NotaDTO mod = cached.notAnchored.First(nota => nota.NotaId == updated.NotaId);
                    mod.NoteContent = updated.NoteContent;
                }
                else//eliminar y acoplar en el otro grupo
                {
                    cached.notAnchored.Add(updated);
                    cached.notAnchored = cached.notAnchored.OrderByDescending(nota => nota.CDT).ToList();
                    cached.anchored.Remove(cached.anchored.First(nota => nota.NotaId == updated.NotaId));
                }
            }
            return cached;
        }
        public NotasCache RemoveFromCache(NotasCache cached, SinglePropJson json)
    {
        if (cached.anchored.Any(nota => nota.NotaId == int.Parse(json.Value)))
        {
            cached.anchored.Remove(cached.anchored.Where(nota => nota.NotaId == int.Parse(json.Value)).First());
        }
        else
        {
            cached.notAnchored.Remove(cached.notAnchored.Where(nota => nota.NotaId == int.Parse(json.Value)).First());
        }
        return cached;
    }
    }
    
}