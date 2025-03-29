using Lab_1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace Lab_1.CustomValidation
{
    public class UniqueNameAttribute : ValidationAttribute
    {
      
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var _dbContext = validationContext.GetRequiredService<UniversityDbContext>();
          
            string name = value as string;
            if (name == null)
            {
                return new ValidationResult("Name is required");
            }

            var entity = validationContext.ObjectInstance;
            var entityType = entity.GetType();
            var idProperty = entityType.GetProperty("Id");
            int entityId = (idProperty != null) ? (int)idProperty.GetValue(entity)! : 0;
          
            bool isDuplicate = _dbContext.Departments
           .AsNoTracking()
           .Any(d => d.Name == name && d.Id != entityId);

            if (isDuplicate)
            {
                return new ValidationResult(ErrorMessage ?? $"Department '{name}' already exists.");
            }
           
            return ValidationResult.Success;
        }

    }
}
