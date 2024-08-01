using Invoices.Data.Models.Enums;
using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DataProcessor.ImportDto
{
    public class InvoicesImportDto
    {
        [Required]
        [Range(1_000_000_000, 1_500_000_000)]
        public int Number { get; set; }
        [Required]
        public string IssueDate { get; set; }
        [Required]
        public string DueDate { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int CurrencyType { get; set; }
        [Required]
        public int ClientId { get; set; }
    }
}
