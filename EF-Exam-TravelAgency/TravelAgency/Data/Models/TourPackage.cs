using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Data.Models
{
    public class TourPackage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string PackageName { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<TourPackageGuide> TourPackagesGuides { get; set; }
    }
}
