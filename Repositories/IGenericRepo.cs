using NotasProject.Models.Config;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotasProject.Repoositories
{
    public interface IGenericRepo<T> : IDisposable where T : class
    {
        PersistedState Create(T entity);
        PersistedState Remove(T entity);
        PersistedState CreateMany(List<T> entity);
        PersistedState RemoveMany(List<T> entity);
        IQueryable<T> BuildQuery();
        void SetEntityState(T entity, EntityState state);
        void Dispose();
        PersistedState TrySaveChanges();
        void LogException(Exception ex);
    }
}
