using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suffer_Travels.Models
{
    [NotMapped]
    public class OTP
    {
        [StringLength(6, ErrorMessage = "Otp not valid", MinimumLength = 6)]
        [Required(ErrorMessage = "Otp cannot be empty")]
        public string otp { get; set; }
    }
}
