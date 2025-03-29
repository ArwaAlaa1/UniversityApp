namespace Lab_1.Dtos
{
    public class StudentSendDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Age { get; set; }
        public string Address { get; set; }
        public int DeptId { get; set; }
        public DateOnly DOB { get; set; }
        public List<string> Skills { get; set; }
        public IFormFile Image { get; set; }
    }
}
