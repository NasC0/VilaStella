using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace VilaStella.Data.Common.Repositories
{
    public class GenericRepository<T> : IGenericRepositoy<T> where T : class
    {
        private DbContext context;

        public GenericRepository(DbContext context)
        {
            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }

        protected DbContext Context
        {
            get
            {
                return this.context;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Db Context value cannot be null");
                }

                this.context = value;
            }
        }

        protected IDbSet<T> DbSet { get; private set; }

        public virtual IQueryable<T> All()
        {
            return this.DbSet.AsQueryable();
        }

        public T GetById(int id)
        {
            return this.DbSet.Find(id);
        }

        public T Find(object id)
        {
            return this.DbSet.Find(id);
        }

        public void Add(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Added);
            this.DbSet.Add(entity);
        }

        public void Update(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Modified);
        }

        public virtual void Delete(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Deleted);
        }

        public void Delete(int id)
        {
            var entity = this.GetById(id);
            this.Delete(entity);
        }

        public void Detach(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Detached);
        }

        public virtual int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }

        protected virtual void ChangeEntityState(T entry, EntityState state)
        {
            var entity = this.Context.Entry(entry);
            if (entity.State == EntityState.Detached)
            {
                this.DbSet.Attach(entry);
            }

            entity.State = state;
        }
    }
}
