using System;
using System.Data;
using Microsoft.Practices.Unity;
using UnitOfWork.Abstractions;
using UnitOfWork.Tests.Entities;
using Xunit;

namespace UnitOfWork.Tests
{
    public class UnitOfWorkTests : IDisposable
    {
        private readonly IUnityContainer _container;

        public UnitOfWorkTests()
        {
            _container = TestBootstrapper.RegisterTypes(new UnityContainer());
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        [Fact]
        public void CanGetRepositoryFromUnitOfWork()
        {
            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                var testsRepository = unitOfWork.Repository<TestEntity>();

                Assert.NotNull(testsRepository);
            }
        }

        [Fact]
        public void CanAddDataWithoutPersisting()
        {
            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                var testsRepository = unitOfWork.Repository<TestEntity>();

                var testdata = $"Test{Guid.NewGuid()}";

                testsRepository.Add(new TestEntity {Data = testdata});

                var entity = testsRepository.FirstOrDefault(d => d.Data == testdata);

                Assert.Null(entity);
            }
        }

        [Fact]
        public void CanAddDataWithPersistence()
        {
            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                var testsRepository = unitOfWork.Repository<TestEntity>();

                var testdata = $"Test{Guid.NewGuid()}";

                testsRepository.Add(new TestEntity {Data = testdata});

                unitOfWork.Commit();

                var entity = testsRepository.FirstOrDefault(d => d.Data == testdata);

                Assert.NotNull(entity);
            }
        }

        [Fact]
        public void CanAddDataWithTransaction()
        {
            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                var testsRepository = unitOfWork.Repository<TestEntity>();

                unitOfWork.BeginTransaction(IsolationLevel.ReadUncommitted);

                var testdata = $"Test{Guid.NewGuid()}";

                testsRepository.Add(new TestEntity {Data = testdata});

                unitOfWork.Commit();

                unitOfWork.CommitTransaction();

                var entity = testsRepository.FirstOrDefault(d => d.Data == testdata);

                Assert.NotNull(entity);
            }
        }

        [Fact]
        public void CanCancelAddDataWithRollbackTransaction()
        {
            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                var testsRepository = unitOfWork.Repository<TestEntity>();

                unitOfWork.BeginTransaction(IsolationLevel.ReadUncommitted);

                var testdata = $"Test{Guid.NewGuid()}";

                testsRepository.Add(new TestEntity {Data = testdata});

                unitOfWork.Commit();

                unitOfWork.Rollback();

                var entity = testsRepository.FirstOrDefault(d => d.Data == testdata);

                Assert.Null(entity);
            }
        }
    }
}