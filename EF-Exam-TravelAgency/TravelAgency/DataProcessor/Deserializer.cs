using Castle.Components.DictionaryAdapter;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            var customersDto = XmlHelper.DeserializeFromString<CustomerImportDto[]>(xmlString, "Customers");

            StringBuilder sb = new StringBuilder();
            List<Customer> customers = new List<Customer>();

            foreach (var c in customersDto)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (customers.Any(cust => cust.FullName == c.FullName)
                    || customers.Any(cust => cust.Email == c.Email)
                    || customers.Any(cust => cust.PhoneNumber == c.PhoneNumber))
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }
                customers.Add(new Customer()
                {
                    PhoneNumber = c.PhoneNumber,
                    FullName = c.FullName,
                    Email = c.Email,
                });
                sb.AppendLine(string.Format(SuccessfullyImportedCustomer, c.FullName));
            }
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            var bookingsDto = JsonConvert.DeserializeObject<BookingImportDto[]>(jsonString);

            List<Booking> bookingsToAdd = new();
            StringBuilder sb = new StringBuilder();
            foreach (var b in bookingsDto)
            {
                if (!DateTime.TryParseExact(b.BookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Customer? customer = context.Customers.FirstOrDefault(c=>c.FullName == b.CustomerName);
                if (customer is null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                TourPackage tourPackage = context.TourPackages.FirstOrDefault(t=>t.PackageName == b.TourPackageName);
                if (tourPackage is null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Booking booking = new Booking()
                {
                    BookingDate = parsedDate,
                    Customer = customer,
                    TourPackage = tourPackage
                };
                bookingsToAdd.Add(booking);
                sb.AppendLine(string.Format(SuccessfullyImportedBooking,tourPackage.PackageName,parsedDate.ToString("yyyy-MM-dd")));
            }
            context.AddRange(bookingsToAdd);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
