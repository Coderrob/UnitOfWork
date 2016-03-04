using System.ComponentModel.DataAnnotations.Schema;

namespace UnitOfWork.Data.DataContext
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }

    public enum ObjectState
    {
        Unchanged,
        Added,
        Modified,
        Deleted
    }
}