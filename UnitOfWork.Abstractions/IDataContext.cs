using System;

namespace UnitOfWork.Abstractions
{
    public interface IDataContext : IDisposable
    {
        Guid InstanceId { get; }
        int SaveChanges();
        void SyncObjectState(object entity);
    }
}