using System.Data.Entity;
using UnitOfWork.Tests.Entities;

namespace UnitOfWork.Tests.TestData
{
    public class TestDatabaseContext : DataContext
    {
        public TestDatabaseContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestEntity>().ToTable("Tests");
        }
    }
}