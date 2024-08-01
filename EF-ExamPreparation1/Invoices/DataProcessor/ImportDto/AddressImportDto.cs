using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Address")]
    public class AddressImportDto
    {
        [MinLength(10)]
        [MaxLength(20)]
        [Required]
        [XmlElement("StreetName")]
        public string StreetName { get; set; }
        [Required]
        [XmlElement("StreetNumber")]
        public int StreetNumber { get; set; }
        [Required]
        [XmlElement("PostCode")]
        public string PostCode { get; set; }
        [MinLength(5)]
        [MaxLength(15)]
        [Required]
        [XmlElement("City")]
        public string City { get; set; }

        [MinLength(5)]
        [MaxLength(15)]
        [Required]
        [XmlElement("Country")]
        public string Country { get; set; }
    }
}