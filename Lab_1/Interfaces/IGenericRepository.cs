using Lab_1.Models;
using Microsoft.Build.Framework;
using System.Runtime.CompilerServices;

namespace Lab_1.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(int id);
        void Add(T entity);
        void Edit(T entity);
        void Remove(T entity);
   


    }
 
}
