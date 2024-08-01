using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DataProcessor.ImportDto
{
    public class ProductsImport
    {
        [MinLength(9)]
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        [Range(5.00, 1000.00)]
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryType { get; set; }
        public int[] Clients { get; set; }
    }
}
