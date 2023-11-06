using System.ComponentModel.DataAnnotations;

namespace StreamingServiceApp.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [UIHint("password")]
        public string Password { get; set; }
    }
}
