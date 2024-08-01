using Invoices.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        [Range(5.00, 1000.00)]
        [Required]
        public decimal Price { get; set; }
        [Required] 
        public CategoryType CategoryType { get; set; }
        public ICollection<ProductClient> ProductsClients { get; set; }
    }
}
