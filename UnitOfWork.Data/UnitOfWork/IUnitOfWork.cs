using System;
using System.Data;
using UnitOfWork.Data.DataContext;

namespace UnitOfWork.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : IObjectState;
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        int Commit();
        bool CommitTransaction();
        void Rollback();
        void Dispose(bool disposing);
    }
}