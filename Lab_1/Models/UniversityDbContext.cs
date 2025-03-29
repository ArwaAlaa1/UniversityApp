using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab_1.Models
{
    public class UniversityDbContext: IdentityDbContext<IdentityUser>
    {

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public UniversityDbContext(DbContextOptions<UniversityDbContext> options):base(options)
        {
            
        }
    }
}
