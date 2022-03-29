using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suffer_Travels.Models
{
    [NotMapped]
    public class Register
    {
        [Required(ErrorMessage = "Email address cannot be empty")]
        public string Email { get; set; }
        [StringLength(6, ErrorMessage = "Otp not valid", MinimumLength = 6)]
        public string Otp { get; set; }
        [Required(ErrorMessage = "Password cannot be empty")]
        [StringLength(20, ErrorMessage = "Password minimum 8 character", MinimumLength = 8)]
        [RegularExpression("[0-9a-zA-Z]+", ErrorMessage = "Password must match the pattern")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Re-type password cannot be empty")]
        public string RePassword { get; set; }
        public Boolean RememberMe { get; set; } = false;
    }
}
