﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Client")]
    public class ClientsWithInvoicesDto
    {
        [XmlAttribute("InvoicesCount")]
        public int InvoicesCount { get; set; }
        [XmlElement("ClientName")]
        public string ClientName { get; set; }
        [XmlElement("VatNumber")]
        public string VatNumber { get; set; }
        [XmlArray("Invoices")]
        public InvoicDto[] Invoices { get; set; }
    }

    [XmlType("Invoice")]
    public class InvoicDto
    {
        [XmlElement("InvoiceNumber")]
        public int InvoiceNumber { get; set; }
        [XmlElement("InvoiceAmount")]
        public decimal InvoiceAmount { get; set; }
        [XmlElement("DueDate")]
        public string DueDate { get; set; }
        [XmlElement("Currency")]
        public string Currency { get; set; }
    }
}
