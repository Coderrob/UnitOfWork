using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using UnitOfWork.Data.DataContext;

namespace UnitOfWork.Data.UnitOfWork.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUnityContainer _container;
        private readonly IDataContext _dataContext;
        private bool _disposed;
        private ObjectContext _objectContext;
        private Dictionary<string, object> _repositories;
        private DbTransaction _transaction;

        public UnitOfWork(
            IUnityContainer container,
            IDataContext dataContext)
        {
            _container = container;
            _dataContext = dataContext;
        }

        public IRepository<T> Repository<T>() where T : IObjectState
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<string, object>();
            }

            var type = typeof (T).Name;

            if (_repositories.ContainsKey(type))
            {
                return (IRepository<T>) _repositories[type];
            }

            var repository = _container.Resolve<IRepository<T>>();

            _repositories.Add(type, repository);

            return repository;
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            _objectContext = ((IObjectContextAdapter) _dataContext).ObjectContext;

            if (_objectContext.Connection.State != ConnectionState.Open)
            {
                _objectContext.Connection.Open();
            }

            _transaction = _objectContext.Connection.BeginTransaction();
        }

        public int Commit()
        {
            Debug.WriteLine("Commit on instance " + _dataContext.InstanceId);

            return _dataContext.SaveChanges();
        }

        public bool CommitTransaction()
        {
            Debug.WriteLine("Commit transactions on instance " + _dataContext.InstanceId);

            _transaction.Commit();
            return true;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            ((DataContext.DataContext) _dataContext).SyncObjectsStatePostCommit();
        }

        public void Dispose()
        {
            if (_objectContext != null && _objectContext.Connection.State == ConnectionState.Open)
            {
                Debug.WriteLine("Closing connection for context " + _dataContext.InstanceId);

                _objectContext.Connection.Close();
            }

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            Debug.WriteLine("Closing context " + _dataContext.InstanceId);

            if (!_disposed && disposing)
            {
                _dataContext.Dispose();
            }

            _disposed = true;
        }
    }
}