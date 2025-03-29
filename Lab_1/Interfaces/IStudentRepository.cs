
using Lab_1.Models;

namespace Lab_1.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudentsWithDept();
        Task<Student?> GetStudentWithDept(int id);
        Task<IEnumerable<Student?>> GetStudentsWithName(string name);
    }
}