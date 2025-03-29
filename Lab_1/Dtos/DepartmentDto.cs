namespace Lab_1.Dtos
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ManagerName { get; set; }
        public List<string> StudentNames { get; set; }
        public int CountOfStudents { get; set; }
        public string Message { get; set; }

    }
}
