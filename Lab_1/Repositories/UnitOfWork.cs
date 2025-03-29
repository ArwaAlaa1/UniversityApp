using Lab_1.Interfaces;
using Lab_1.Models;
using System.Collections;

namespace Lab_1.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UniversityDbContext _dbContext;
        private Hashtable _repositories;
        public UnitOfWork(UniversityDbContext dbContext)
        {
           _dbContext = dbContext;
            _repositories= new Hashtable();
        }
        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
           var key =typeof(T).Name;
            if (!_repositories.ContainsKey(key))
            {
                var repository = new GenericRepository<T>(_dbContext) ;
                _repositories.Add(key, repository);
            }
           return _repositories[key] as IGenericRepository<T>;
        }
        public async Task<int> CompleteAsync()
        {
           return await _dbContext.SaveChangesAsync();
           
        }

        public ValueTask DisposeAsync()
        {
           return  _dbContext.DisposeAsync();
        }

     
    }
}
