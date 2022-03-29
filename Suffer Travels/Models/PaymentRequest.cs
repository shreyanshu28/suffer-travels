using System.ComponentModel.DataAnnotations;

namespace Suffer_Travels.Models
{
    public class PaymentRequest
    {
        [Required]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Name should only contain alphabets!")]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^[6-9][0-9]{9,9}", ErrorMessage = "Please enter valid contact number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
