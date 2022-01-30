using System.ComponentModel.DataAnnotations;

namespace Suffer_Travels.Models
{
    public class Role
    {
        [Key]
        public UInt32 Id { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
