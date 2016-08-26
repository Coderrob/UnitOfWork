# UnitOfWork

There are a number of [implementation examples of using the Repository pattern and Unit of Work](http://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application). 
I always found that the impelmentation always felt very rigid to me. I wanted to create a generic IRepository and IUnitOfWork that 
could use the Unit of Work pattern in a generic way for abstraction from any database.

I've used EntityFramework on many bootstrap projects and have had to just re-implement the same
logic over and over again setting up DbContexts. So much that I created this project to quickly 
pull down the code, change the project names, and include it as a generic data access library.

This project contains an abstractions project called UnitOfWork.Abstractions that contains the
interfaces for IUnitOfWork, IRepository, and for those using EntityFramework the IDataContext. The 
implementations for these abstractions are in the UnitOfWork.EntityFramework project where they've
been implemented with EF backing classes.

THe core to these abstractions are the IUnitOfWork and IRepository interfaces below:

```
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : IObjectState;
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        int Commit();
        bool CommitTransaction();
        void Rollback();
        void Dispose(bool disposing);
    }
```

```
    public interface IRepository<T> where T : IObjectState
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
        bool Any(Expression<Func<T, bool>> predicate);
        T First(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
```

In the UnitOfWork.Tests project you'll find a few xUnit tests around resolving IRepository classes,
and how to use the IUnitOfWork against an EntityFramework DbContext.

```
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
```

Once bootstrapped in your project you can use direct injection of these classes into any controller or services for any projects.

```
    public class SomeController 
    {
        private readonly IUnitOfWork _unitOfWork;

        SomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var repository = _unitOfWork.Repository<SomeEntity>();

            return View(repository.GetAll());
        }

        [HttpPut]
        public ActionResult Put(SomeData someData)
        {
            var repository = _unitOfWork.Repository<SomeEntity>();

            . . .  // Convert someData to SomeEntity via AutoMapper perhaps?            

            repository.Add(someDataEntity)

            _unitOfWork.Commit();

            return RedirectToAction("Get");
        }
    }
```