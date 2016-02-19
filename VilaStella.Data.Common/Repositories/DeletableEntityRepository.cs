using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VilaStella.Data.Common.Models;

namespace VilaStella.Data.Common.Repositories
{
    public class DeletableEntityRepository<T> : GenericRepository<T>, IDeletableRepository<T> where T : class, IDeletableEntity
    {
        public DeletableEntityRepository(DbContext context)
            : base(context)
        {
        }

        public override IQueryable<T> All()
        {
            return this.DbSet.Where(e => !e.IsDeleted).AsQueryable();
        }

        public IQueryable<T> AllWithDeleted()
        {
            return this.DbSet.AsQueryable();
        }

        public override void Delete(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.Now;

            this.ChangeEntityState(entity, EntityState.Modified);
        }

        public void ActualDelete(T entity)
        {
            base.Delete(entity);
        }
    }
}
