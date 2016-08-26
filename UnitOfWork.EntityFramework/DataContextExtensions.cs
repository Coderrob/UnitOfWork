using System;
using System.Data.Entity;
using UnitOfWork.Abstractions;

namespace UnitOfWork
{
    public static class DataContextExtensions
    {
        public static EntityState ConvertState(this ObjectState objectState)
        {
            switch (objectState)
            {
                case ObjectState.Added:
                    return EntityState.Added;

                case ObjectState.Modified:
                    return EntityState.Modified;

                case ObjectState.Deleted:
                    return EntityState.Deleted;

                case ObjectState.Unchanged:
                    return EntityState.Unchanged;

                default:
                    throw new ArgumentOutOfRangeException(nameof(objectState));
            }
        }

        public static ObjectState ConvertState(this EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Detached:
                    return ObjectState.Unchanged;

                case EntityState.Unchanged:
                    return ObjectState.Unchanged;

                case EntityState.Added:
                    return ObjectState.Added;

                case EntityState.Deleted:
                    return ObjectState.Deleted;

                case EntityState.Modified:
                    return ObjectState.Modified;

                default:
                    throw new ArgumentOutOfRangeException(nameof(entityState));
            }
        }
    }
}