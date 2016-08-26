using System.ComponentModel.DataAnnotations.Schema;
using UnitOfWork.Abstractions;

namespace UnitOfWork
{
    public abstract class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}