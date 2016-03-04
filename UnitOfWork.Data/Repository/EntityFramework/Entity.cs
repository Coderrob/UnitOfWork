using System.ComponentModel.DataAnnotations.Schema;
using UnitOfWork.Data.DataContext;

namespace UnitOfWork.Data.Repository.EntityFramework
{
    public abstract class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}