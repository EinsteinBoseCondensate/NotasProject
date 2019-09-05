using NotasProject.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotasProject.Repoositories
{
    public interface IGenericRepo<T> : IDisposable where T : class
    {
        void Create(T entity);
        void Remove(T entity);
        void CreateMany(List<T> entity);
        void RemoveMany(List<T> entity);
        IQueryable<T> BuildQuery();
        void Dispose(bool disposed);
        PersistedState TrySaveChanges();
        void LogException();
    }
}
