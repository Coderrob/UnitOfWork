using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using UnitOfWork.Abstractions;

namespace UnitOfWork
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly IDataContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(IDataContext context)
        {
            _context = context;

            var dbContext = context as DbContext;

            if (dbContext == null) return;

            _dbSet = dbContext.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public IQueryable<T> Query()
        {
            return _dbSet;
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.First(predicate);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public void Add(T entity)
        {
            ((IObjectState) entity).ObjectState = ObjectState.Added;
            _dbSet.Add(entity);
            _context.SyncObjectState(entity);
        }

        public void Delete(T entity)
        {
            ((IObjectState) entity).ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        public void Update(T entity)
        {
            ((IObjectState) entity).ObjectState = ObjectState.Modified;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }
    }
}