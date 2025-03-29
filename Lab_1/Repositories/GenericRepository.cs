using Lab_1.Interfaces;
using Lab_1.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab_1.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
       protected readonly UniversityDbContext _dbContext;

        public GenericRepository(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(T entity)
        {
           _dbContext.Add(entity);
           
        }

        public void Edit(T entity)
        {
           _dbContext.Update(entity);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
           var result = await _dbContext.Set<T>().Where(t=>t.IsDeleted==false).ToListAsync();
            return result;
        }

        public async Task<T?> GetById(int id)
        {
           var result = await _dbContext.Set<T>().FindAsync(id);
            return result;
        }

        public void Remove(T entity)
        {
            _dbContext.Remove(entity);
        }
    }
}
