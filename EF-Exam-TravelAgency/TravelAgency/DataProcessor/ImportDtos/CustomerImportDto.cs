using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ImportDtos
{
    [XmlType("Customer")]
    public class CustomerImportDto
    {
        [MaxLength(13)]
        [MinLength(13)]
        [RegularExpression(@"^\+\d{12}$")]
        [XmlAttribute("phoneNumber")]
        public string PhoneNumber { get; set; }
        [MinLength(4)]
        [MaxLength(60)]
        [XmlElement("FullName")]
        [Required]
        public string FullName { get; set; }
        [Required]
        [MinLength(6)]
        [XmlElement("Email")]
        [MaxLength(50)]
        public string Email { get; set; }
    }
}
