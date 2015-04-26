using System;
using System.Linq;

namespace VilaStella.Data.Common.Repositories
{
    public interface IGenericRepositoy<T> : IDisposable where T : class
    {
        IQueryable<T> All();

        T GetById(int id);

        T Find(object id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(int id);

        void Detach(T entity);

        int SaveChanges();
    }
}
