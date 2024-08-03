using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("Guide")]
    public class GuideExportDto
    {
        [XmlElement("FullName")]
        public string FullName { get; set; }
        [XmlArray("TourPackages")]
        public TourPackageExportDto[] TourPackages { get; set; }
    }

    [XmlType("TourPackage")]
    public class TourPackageExportDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Description")]
        public string Description { get; set; }
        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}
