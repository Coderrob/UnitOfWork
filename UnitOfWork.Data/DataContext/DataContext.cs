using System;
using System.Data.Entity;
using System.Diagnostics;

namespace UnitOfWork.Data.DataContext
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            InstanceId = Guid.NewGuid();
            Debug.WriteLine("Creating instance " + InstanceId);
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public Guid InstanceId { get; }

        public override int SaveChanges()
        {
            Debug.WriteLine("Saving to instance " + InstanceId);
            SyncObjectsStatePreCommit();
            var changes = base.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }

        public void SyncObjectState(object entity)
        {
            Entry(entity).State = StateHelper.ConvertState(((IObjectState) entity).ObjectState);
        }

        public new DbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        private void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState) dbEntityEntry.Entity).ObjectState);
            }
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                ((IObjectState) dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
            }
        }
    }
}