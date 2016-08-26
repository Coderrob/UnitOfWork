using System;
using Microsoft.Practices.Unity;
using UnitOfWork.Abstractions;
using UnitOfWork.Tests.Entities;
using Xunit;

namespace UnitOfWork.Tests
{
    public class RepositoryTests : IDisposable
    {
        private readonly IUnityContainer _container;

        public RepositoryTests()
        {
            _container = TestBootstrapper.RegisterTypes(new UnityContainer());
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        [Fact]
        public void CanResolveRepository()
        {
            Assert.NotNull(_container.Resolve<IRepository<TestEntity>>());
        }
    }
}