using System.ComponentModel.DataAnnotations;

namespace StreamingServiceApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [UIHint("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [UIHint("password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
