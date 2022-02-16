using System.ComponentModel.DataAnnotations;

namespace Suffer_Travels.Models
{
    public class Photo
    {
        [Key]
        public UInt32 PId { get; set; }

        public string ImagePath { get; set; } = "";

        public string Description { get; set; } = "";
    }
}
