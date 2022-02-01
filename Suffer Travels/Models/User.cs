
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suffer_Travels.Models
{
    public class User
    {
        [Key]
        public UInt32 UId { get; set; }
        [Required]
        public string Fname { get; set; }
        public string Mname { get; set; } = "";
        [Required]
        public string Lname { get; set; }
        public char Gender { get; set; } = 'M';
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [Phone(ErrorMessage = "Enter a valid phone number")]
        public long ContactNo { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Username should have 3 to 50 characters only", MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Password minimum 8 character", MinimumLength = 8)]
        public string Password { get; set; }
        public string ProfilePhoto { get; set; }
        public Boolean IsActive { get; set; } = true;
        [ForeignKey("Role")]
        public UInt32 RoleId { get; set; }  

    }
}
