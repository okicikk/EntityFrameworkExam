using Newtonsoft.Json;
using System.Text.Json.Nodes;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ExportDtos;
using TravelAgency.Data.Models.Enums;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            var gudeisWithSpanishLanguageDto = context.Guides
                .Where(g => g.Language == Language.Spanish)
                .Select(g => new GuideExportDto()
                {
                    FullName = g.FullName,
                    TourPackages = g.TourPackagesGuides.Select(g => new TourPackageExportDto()
                    {
                        Name = g.TourPackage.PackageName,
                        Description = g.TourPackage.Description,
                        Price = g.TourPackage.Price,
                    })
                    .OrderByDescending(p=>p.Price)
                    .ThenBy(p => p.Name)
                    .ToArray()
                })
                .OrderByDescending(tp => tp.TourPackages.Count())
                .ThenBy(tp => tp.FullName)
                .ToList();
            return XmlHelper.SerializeToString(gudeisWithSpanishLanguageDto, "Guides");
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            var customersToExport = context.Customers
                .Where(b => b.Bookings.Any(b => b.TourPackage.PackageName == "Horse Riding Tour"))
                .Select(c => new
                {
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Bookings = c.Bookings.Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
                    .OrderBy(b => b.BookingDate)
                    .Select(b => new
                    {
                        TourPackageName = b.TourPackage.PackageName,
                        Date = b.BookingDate.ToString("yyyy-MM-dd")
                    })
                    .ToArray()
                })
                .OrderByDescending(c => c.Bookings.Count())
                .ThenBy(c => c.FullName)
                .ToList();
            return JsonConvert.SerializeObject(customersToExport, Formatting.Indented);

        }
    }
}
