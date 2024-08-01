namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using AutoMapper;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            var clientsDto = XmlHelper.DeserializeFromString<ClientImportDto[]>(xmlString, "Clients");

            StringBuilder sb = new StringBuilder();
            var clientsToImport = new List<Client>();

            foreach (var c in clientsDto)
            {
                var clientAddresses = new List<Address>();
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                foreach (var a in c.Addresses)
                {
                    if (!IsValid(a))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    clientAddresses.Add(new Address()
                    {
                        StreetName = a.StreetName,
                        StreetNumber = a.StreetNumber,
                        PostCode = a.PostCode,
                        City = a.City,
                        Country = a.Country
                    });
                }
                clientsToImport.Add(new Client()
                {
                    Name = c.Name,
                    NumberVat = c.NumberVat,
                    Addresses = clientAddresses
                });
                sb.AppendLine(string.Format(SuccessfullyImportedClients, c.Name));
            }
            context.Clients.AddRange(clientsToImport);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            var invoicesDto = JsonConvert.DeserializeObject<InvoicesImportDto[]>(jsonString);

            List<Invoice> invoicesToAdd = new();
            StringBuilder sb = new StringBuilder();

            foreach (var i in invoicesDto)
            {
                if (!IsValid(i))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (!DateTime.TryParseExact(i.IssueDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validIssueDate)
                    || !DateTime.TryParseExact(i.DueDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validDueDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (DateTime.Compare(validIssueDate, validDueDate) >= 0 || !Enum.IsDefined(typeof(CurrencyType), i.CurrencyType))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Client? client = context.Clients.Find(i.ClientId);
                if (client is null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                invoicesToAdd.Add(new Invoice()
                {
                    Number = i.Number,
                    IssueDate = validIssueDate,
                    DueDate = validDueDate,
                    Amount = i.Amount,
                    CurrencyType = (CurrencyType)i.CurrencyType,
                    Client = client
                });
                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, i.Number));
            }
            context.Invoices.AddRange(invoicesToAdd);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            var productsDto = JsonConvert.DeserializeObject<ProductsImport[]>(jsonString);

            StringBuilder sb = new();
            List<Product> productsToBeAdded = new List<Product>();

            foreach (var p in productsDto)
            {
                if (!IsValid(p) || !Enum.IsDefined(typeof(CategoryType), p.CategoryType))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                List<ProductClient> validProductClients = new();
                Product product = new Product()
                {
                    Name = p.Name,
                    Price = p.Price,
                    CategoryType = (CategoryType)p.CategoryType
                };
                foreach (var clientId in p.Clients.Distinct())
                {
                    Client client = context.Clients.Find(clientId);
                    if (client is null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    validProductClients.Add(new()
                    {
                        Product = product,
                        Client = client
                    });
                }
                product.ProductsClients = validProductClients;
                productsToBeAdded.Add(product);
                sb.AppendLine(string.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count));
            }
            context.Products.AddRange(productsToBeAdded);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
