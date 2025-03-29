using Lab_1.Models;

namespace Lab_1.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetDepartmentsWithSudentsAsync();
        Task<Department?> GetOneDeptWithSudentsAsync(int id);
        Task<Department?> GetDepartmentWithNameAsync(string name);
    }
}