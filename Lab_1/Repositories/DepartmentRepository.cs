using Lab_1.Interfaces;
using Lab_1.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab_1.Repositories
{
    public class DepartmentRepository: GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(UniversityDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Department>> GetDepartmentsWithSudentsAsync()
        {
           return await _dbContext.Departments.Include(d => d.Students).Where(d=>d.IsDeleted==false).ToListAsync();

        }

        public async Task<Department?> GetDepartmentWithNameAsync(string name)
        {
            return await _dbContext.Departments.Where(d => d.Name.ToLower() == name.ToLower()).Include(s => s.Students).FirstOrDefaultAsync();
        }

        public async Task<Department?> GetOneDeptWithSudentsAsync(int id )
        {
           return await _dbContext.Departments.Where(d => d.Id == id).Include(s => s.Students).FirstOrDefaultAsync();
        }
    }
}
