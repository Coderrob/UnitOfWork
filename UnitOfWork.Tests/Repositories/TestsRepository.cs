using UnitOfWork.Abstractions;

namespace UnitOfWork.Tests.Repositories
{
    internal class TestsRepository : Repository<Entities.TestEntity>
    {
        public TestsRepository(IDataContext context) : base(context)
        {
        }
    }
}