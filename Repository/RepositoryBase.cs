using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Contracts;
using Entities;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }
        
        public RepositoryBase(RepositoryContext context)
        {
            RepositoryContext = context;
        }

        public IQueryable<T> FindAll()
        {
            return RepositoryContext.Set<T>().AsNoTracking();
        }

        public T FindByKey(Guid key)
        {
            return RepositoryContext.Set<T>().Find(key);
        }

        public bool Exists(Expression<Func<T, bool>> expression)
        {
            return RepositoryContext.Set<T>().Any<T>(expression);
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            RepositoryContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);
        }

    }
}
