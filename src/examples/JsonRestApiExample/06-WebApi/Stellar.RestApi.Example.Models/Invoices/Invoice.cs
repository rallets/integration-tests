using System;
using System.Collections.Generic;
using Stellar.RestApi.Example.Models.Common;

namespace Stellar.RestApi.Example.Models
{
    public abstract class InvoiceBase
    {
        public Guid ClientId { get; set; }

        public Guid OrderId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
    }

    public class Invoice : InvoiceBase, IIdentifier
    {
        public Guid Id { get ; set ; }
    }

    public class GetInvoiceRequest
    {
        public Guid Id { get; set; }
    }

    public class GetInvoiceResponse
    {
        public Invoice Result { get; set; }
    }

    public class GetInvoicesResponse
    {
        public List<Invoice> Result { get; set; }
    }

    public class AddInvoiceRequest : InvoiceBase
    {
    }

    public class AddInvoiceResponse
    {
        public Invoice Result { get; set; }
    }

    public class EditInvoiceRequest: InvoiceBase
    {
    }

    public class EditInvoiceResponse
    {
        public Invoice Result { get; set; }
    }
}
