using Lab_1.Models;

namespace Lab_1.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable 
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> CompleteAsync();

    }
}
