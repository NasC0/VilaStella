using System.Linq;

namespace VilaStella.Data.Common.Repositories
{
    public interface IDeletableRepository<T> : IGenericRepositoy<T> where T : class
    {
        IQueryable<T> AllWithDeleted();

        void ActualDelete(T entity);
    }
}
