namespace Lab_1.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }=false;
        public bool IsActive { get; set; }=true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}