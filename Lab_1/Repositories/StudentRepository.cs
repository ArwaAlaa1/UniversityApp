using Lab_1.Dtos;
using Lab_1.Interfaces;
using Lab_1.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace Lab_1.Repositories
{
    public class StudentRepository : GenericRepository<Student>,IStudentRepository
    {
        public StudentRepository(UniversityDbContext dbContext):base(dbContext) 
        {
            
        }
        public async Task<IEnumerable<Student>> GetAllStudentsWithDept()
        {
            return await _dbContext.Students.Include(s => s.Dept).ToListAsync();
        }

        public async Task<IEnumerable<Student?>> GetStudentsWithName(string name)
        {
           return await _dbContext.Students.Include(d => d.Dept).Where(n => n.Name == name).ToListAsync();
        }

        public async Task<Student?> GetStudentWithDept(int id)
        {
            return await _dbContext.Students.AsNoTracking().Include(s => s.Dept).FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
