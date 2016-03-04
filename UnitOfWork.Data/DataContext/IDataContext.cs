using System;

namespace UnitOfWork.Data.DataContext
{
    public interface IDataContext : IDisposable
    {
        Guid InstanceId { get; }
        int SaveChanges();
        void SyncObjectState(object entity);
    }
}