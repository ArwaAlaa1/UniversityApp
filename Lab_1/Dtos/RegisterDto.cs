using System.ComponentModel.DataAnnotations;

namespace Lab_1.Dtos
{
    public class RegisterDto
    {
      
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
