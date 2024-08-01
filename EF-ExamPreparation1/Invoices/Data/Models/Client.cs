using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        [Required]
        [MaxLength(15)]
        public string NumberVat { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<ProductClient> ProductsClients { get; set; }
    }
}
