using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyMap.Models.Entities
{
    public class Entity
    {
        [Display(Name = "Date")]
        public DateTime DateRegistered { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public string? ImagePath1 { get; set; }

        [NotMapped]
        public string? ImagePath2 { get; set; }

        [NotMapped]
        public string? ImagePath3 { get; set; }

        [NotMapped]
        public string? ImagePath4 { get; set; }

        [NotMapped]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
