using System.ComponentModel.DataAnnotations;

namespace Boardgames.Data.Models
{
    public class Creator
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(7)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(7)]
        [Required]
        public string LastName { get; set; }

        public ICollection<Boardgame> Boardgames { get; set; }
    }
}