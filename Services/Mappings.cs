using NotasProject.Models;
using NotasProject.Models.Config;
using NotasProject.Models.DTOs;
using NotasProject.Models.Jsons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotasProject.Services
{
    public static class Mappings
    {
        public static Nota ConvertToNota(NotaDTO dto, ApplicationUser user)
        {
            AES_Result res = CryptoService.AES_Encrypt(dto.NoteContent);
            return new Nota()
            {
                NotaId = dto.NotaId,
                LUDT = DateTime.Now,
                CDT = DateTime.Now,
                NoteContent = res.CipherData,
                Nonce = res.Nonce,
                Anchor = dto.Anchor,
                User = user
            };
        }
        public static NotaDTO ConvertFromNota(Nota nota)
        {
            return new NotaDTO()
            {
                NotaId = nota.NotaId,
                LUDT = nota.LUDT,
                CDT = nota.CDT,
                Anchor = nota.Anchor,
                NoteContent = CryptoService.AES_Decrypt(nota.NoteContent, nota.Nonce)
            };
        }
        public static Nota ConvertToNota(CreateNotaDTO dto, ApplicationUser user)
        {
            AES_Result res = CryptoService.AES_Encrypt(dto.NoteContent);
            return new Nota()
            {
                LUDT = DateTime.Now,
                CDT = DateTime.Now,
                NoteContent = res.CipherData,
                Nonce = res.Nonce,
                Anchor = dto.Anchor,
                User = user
            };
        }
    }
}