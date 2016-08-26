using System;
using System.IO;
using Microsoft.Practices.Unity;
using UnitOfWork.Abstractions;
using UnitOfWork.Tests.TestData;

namespace UnitOfWork.Tests
{
    public class TestBootstrapper
    {
        public static IUnityContainer RegisterTypes(IUnityContainer container)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData"));

            return container
                .RegisterType<IDataContext, TestDatabaseContext>(new HierarchicalLifetimeManager(), new InjectionConstructor("TestDatabase"))
                .RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager())
                .RegisterType(typeof(IRepository<>), typeof(Repository<>))
                ;
        }
    }
}