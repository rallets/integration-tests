using System;
using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Scenarios.Default
{
    public class EditInvoiceRequestExtended : EditInvoiceRequest
    {
        public Guid Id { get; set; }
    }

    public partial class DefaultScenario
    {
        public void EditInvoice()
        {
            if (EditInvoiceRequest == null)
            {
                PrepareEditInvoiceRequest();
            }

            var api = new StellarTestApi<EditInvoiceRequest, EditInvoiceResponse>(_logger);

            var url = $"{BaseUrl}v1/invoices/{EditInvoiceRequest.Id}";

            api
                .SetRequest(EditInvoiceRequest)
                .Execute(api.RestClient.PostAsync<EditInvoiceResponse>, url, DefaultHeaders);

            EditInvoiceResponse = api.Response.Data.Result;
        }

        public void PrepareEditInvoiceRequest()
        {
            if(GetInvoiceResponse == null)
            {
                GetInvoice();
            }

            EditInvoiceRequest = new EditInvoiceRequestExtended
            {
                Id = GetInvoiceResponse.Result.Id,
                ClientId = GetInvoiceResponse.Result.ClientId,
                OrderId = GetInvoiceResponse.Result.OrderId,
                Description = $"Edit invoice for order {GetInvoiceResponse.Result.ClientId} {DateTime.Now.ToLongDateString()}",
                Amount = GetInvoiceResponse.Result.Amount + 1
            };

        }
    }
}
