using Lab_1.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace Lab_1.Models
{
    public class Department:BaseEntity
    {
       
        
        public string Name { get; set; }
      
        public string Location { get; set; }
        public string ManagerName { get; set; }
        public List<Student> Students { get; set; }
    }
}
