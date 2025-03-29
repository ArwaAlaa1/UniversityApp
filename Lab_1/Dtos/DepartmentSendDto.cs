using Lab_1.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace Lab_1.Dtos
{
    public class DepartmentSendDto
    {
        public int Id { get; set; }
        [UniqueName]
        public string Name { get; set; }
        //[RegularExpression("EG|USA")]
        public string Location { get; set; }
        public string ManagerName { get; set; }
    }
}
