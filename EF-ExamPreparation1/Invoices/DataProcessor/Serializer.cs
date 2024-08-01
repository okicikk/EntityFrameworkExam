namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ExportDto;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            var clients = context.Clients
                .Where(c => c.Invoices.Any() && c.Invoices.Any(i => DateTime.Compare(i.IssueDate, date) >= 0))
                .Select(c => new ClientsWithInvoicesDto
                {
                    ClientName = c.Name,
                    VatNumber = c.NumberVat,
                    InvoicesCount = c.Invoices.Count(),
                    Invoices = c.Invoices
                    .OrderBy(i=>i.IssueDate)
                    .ThenByDescending(i=>i.DueDate)
                    .Select(i => new InvoicDto
                    {
                        InvoiceNumber = i.Number,
                        InvoiceAmount = i.Amount,
                        DueDate = i.DueDate.ToString("MM/dd/yyyy"),
                        Currency = i.CurrencyType.ToString()
                    })
                    .ToArray()
                })
                .OrderByDescending(c => c.Invoices.Count())
                .ThenBy(c => c.ClientName)
                .ToList();

            return XmlHelper.SerializeToString(clients, "Clients");
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var products = context.Products
              .Where(p => p.ProductsClients.Any())
              .Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
              .Select(p => new
              {
                  Name = p.Name,
                  Price = p.Price,
                  Category = p.CategoryType.ToString(),
                  Clients = p.ProductsClients.Select(pc => new
                  {
                      Name = pc.Client.Name,
                      NumberVat = pc.Client.NumberVat
                  })
                  .Where(c => c.Name.Length >= nameLength)
                  .OrderBy(c => c.Name)
                  .ToList()
              })
              .OrderByDescending(p => p.Clients.Count())
              .ThenBy(p => p.Name)
              .Take(5)
              .ToList();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }
    }
}