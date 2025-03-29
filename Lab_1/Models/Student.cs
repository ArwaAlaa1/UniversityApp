using System.ComponentModel.DataAnnotations.Schema;

namespace Lab_1.Models
{
    public class Student:BaseEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public DateOnly DOB { get; set; }
        public string Image { get; set; }
       
        [ForeignKey("Dept")]
        public int? DeptId { get; set; }
        public Department? Dept { get; set; }
    }
}
