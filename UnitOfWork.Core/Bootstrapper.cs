using Microsoft.Practices.Unity;
using UnitOfWork.Data;
using UnitOfWork.Data.DataContext;
using UnitOfWork.Data.Repository.EntityFramework;
using UnitOfWork.Data.UnitOfWork;
using EF = UnitOfWork.Data.UnitOfWork.EntityFramework;

namespace UnitOfWork.Core
{
    public class Bootstrapper
    {
        public IUnityContainer RegisterTypes(IUnityContainer container)
        {
            return container
                .RegisterType<IDataContext, DataContext>(new HierarchicalLifetimeManager(), new InjectionConstructor("DatabaseContext"))
                .RegisterType<IUnitOfWork, EF.UnitOfWork>(new HierarchicalLifetimeManager())
                .RegisterType(typeof (IRepository<>), typeof (Repository<>))
                ;
        }
    }
}