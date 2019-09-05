using NotasProject.Models;
using NotasProject.Models.Config;
using NotasProject.Models.DTOs;
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
        public PersistedState Create(CreateNotaDTO dto, ApplicationUser user)
        {
            return _notasRepo.CreateNote(Mappings.ConvertToNota(dto, user));
        }
        public PersistedState Edit(NotaDTO dto)
        {
            Nota nota = FindById(dto.NotaId);
            AES_Result res = CryptoService.AES_Encrypt(dto.NoteContent);
            nota.NoteContent = res.CipherData;
            nota.Nonce = res.Nonce;
            nota.LUDT = DateTime.Now;
            return _notasRepo.TrySaveChanges();
        }
        public Tuple<List<NotaDTO>, List<NotaDTO>> GetNotasByUserId(string userId)
        {
            List<NotaDTO> notaDtoList = new List<NotaDTO>();
            _notasRepo.BuildQuery().Where(x => x.User.Id == userId).ToList().ForEach(note => notaDtoList.Add(Mappings.ConvertFromNota(note)));
            return Tuple.Create(notaDtoList.Where(note => note.Anchor).OrderByDescending(x => x.CDT).ToList(), notaDtoList.Where(note => !note.Anchor).OrderByDescending(x => x.CDT).ToList());
        }

        public PersistedState Remove(int id)
        {
            return _notasRepo.RemoveNotaById(id);
        }
    }
}