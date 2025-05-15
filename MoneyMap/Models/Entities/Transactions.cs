using System.ComponentModel.DataAnnotations;

namespace MoneyMap.Models.Entities
{
    public class Transactions:Entity
    {
        [Key]
        public Guid IdTransaction { get; set; }

        [MaxLength(500)]
        public required string Description { get; set; }

        [Required]
        public double Amount { get; set; }

        [MaxLength(50)]
        public string FileAttached { get; set; }
        public Guid CategoryId { get; set; }

        public Categories Category { get; set; }
    }
}
