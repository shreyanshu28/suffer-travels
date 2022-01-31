
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
        public string Mname { get; set; }
        [Required]
        public string Lname { get; set; }

        public char Gender { get; set; } = 'M';
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public Int64 ContactNo { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string ProfilePhoto { get; set; }
        public Boolean IsActive { get; set; } = true;
        [ForeignKey("Role")]
        public UInt32 RoleId { get; set; }  

    }
}
