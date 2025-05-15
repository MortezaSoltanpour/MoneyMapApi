using System.ComponentModel.DataAnnotations;

namespace MoneyMap.Models.Entities
{
    public class Users : Entity
    {
        [Key]
        public Guid IdUser { get; set; }
 
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string Fullname { get; set; }
    }
}
