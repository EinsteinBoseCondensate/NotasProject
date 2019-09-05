using NotasProject.Models;
using NotasProject.Models.Config;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace NotasProject.Repoositories
{
    public abstract class GenericRepo<T> where T : class
    {
        public ApplicationDbContext _context;
        private DbSet<T> _set;
        private bool _disposed;
        public GenericRepo(ApplicationDbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
            _disposed = false;
        }
        public GenericRepo()
        {
            _context = new ApplicationDbContext();
            _disposed = false;
        }
        public PersistedState Create(T entity)
        {
            _set.Add(entity);
            return TrySaveChanges();
        }
        public PersistedState Remove(T entity)
        {
            _set.Remove(entity);
            return TrySaveChanges();
        }
        public PersistedState CreateMany(List<T> entities)
        {
            entities.ForEach(entity => { _set.Add(entity); });
            return TrySaveChanges();
        }
        public PersistedState RemoveMany(List<T> entities)
        {
            entities.ForEach(entity => { _set.Remove(entity); });
            return TrySaveChanges();
        }
        public IQueryable<T> BuildQuery()
        {
            return _set.AsQueryable();
        }
        public PersistedState TrySaveChanges()
        {
            try
            {
                _context.SaveChanges();
                return PersistedState.OK;
            }catch(Exception ex)
            {
                LogException(ex);
                return PersistedState.KO;
            }
        }
        public void LogException(Exception ex)
        {
                _context.Set<ActivityLogModel>().Add(
                    new ActivityLogModel() { LogTime = DateTime.Now, Realm = ex.GetType().ToString(), Message = ex.Message });
                _context.SaveChanges();
        }
        public void Dispose()
        {
            if (!_disposed)
            {
                _context.Dispose();
                GC.SuppressFinalize(this);
                _disposed = true;

            }
        }

    }
}
