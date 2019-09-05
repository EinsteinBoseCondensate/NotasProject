using NotasProject.Models;
using NotasProject.Models.Config;
using NotasProject.Repoositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NotasProject.Repositories
{
    public class NotasRepository : GenericRepo<Nota>
    {
        public NotasRepository(ApplicationDbContext context) : base(context) { }

        public PersistedState CreateNote(Nota nota)
        {
             _context.Entry(nota.User).State = EntityState.Modified;//Al sacar el objeto de caché EF no lo tiene trackeado y al hacer Add trata de guardarlo como si fuera nuevo
            return Create(nota);
        }
        public Nota GetNotaById(int id)
        {
            return BuildQuery().SingleOrDefault(nota => nota.NotaId == id);
        }

        public PersistedState RemoveNotaById(int id)
        {
            return Remove(GetNotaById(id));
        }

    }
}