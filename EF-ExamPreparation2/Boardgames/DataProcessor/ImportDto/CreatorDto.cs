using Boardgames.Data.Models;
using Boardgames.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class CreatorDto
    {


        [MinLength(2)]
        [MaxLength(7)]
        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [MinLength(2)]
        [MaxLength(7)]
        [XmlElement("LastName")]
        public string LastName { get; set; }

        [XmlArray("Boardgames")]
        public BoardgameDto[] Boardgames { get; set; }
    }

    [XmlType("Boardgame")]
    public class BoardgameDto
    {
        [Required]
        [MinLength(10)]
        [MaxLength(20)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Rating")]
        [Range(1, 10.00)]
        [Required]
        public double Rating { get; set; }

        [XmlElement("YearPublished")]
        [Range(2018, 2023)]
        [Required]
        public int YearPublished { get; set; }
        [Required]
        [XmlElement("CategoryType")]
        public int CategoryType { get; set; }

        [Required]
        [XmlElement("Mechanics")]

        public string Mechanics { get; set; }

    }
}
