using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suffer_Travels.Models
{
    [NotMapped]
    public class Register
    {
        [Required(ErrorMessage = "Email address cannot be empty")]
        public string email { get; set; }
        
        [Required]
        [StringLength(20, ErrorMessage = "Password minimum 8 character", MinimumLength = 8)]
        [RegularExpression("[0-9a-zA-Z]+", ErrorMessage = "Password must match the pattern")]
        public string password { get; set; }
        
        [Required(ErrorMessage = "Retype password cannot be empty")]
        public string rePassword { get; set; }
    }
}
