using System.ComponentModel.DataAnnotations;

namespace MoneyMap.Models.Entities
{
    public class Categories : Entity
    {
        [Key]
        public Guid IdCategory { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }

        [Required]
        public bool IsInput { get; set; }

        public List<Transactions> Transaction { get; set; }
    }
}
