using Stellar.IntegrationTests.Core.Helpers;
using Stellar.RestApi.Example.Models;
using System;

namespace Stellar.RestApi.Tests.Scenarios.Default
{
    public partial class DefaultScenario
    {
        public void AddInvoice()
        {
            if (AddInvoiceRequest == null)
            {
                PrepareAddInvoiceRequest();
            }

            var api = new StellarTestApi<AddInvoiceRequest, AddInvoiceResponse>(_logger);

            var url = $"{BaseUrl}v1/invoices/";

            api
                .SetRequest(AddInvoiceRequest)
                .Execute(api.RestClient.PostAsync<AddInvoiceResponse>, url, DefaultHeaders);

            AddInvoiceResponse = api.Response.Data.Result;
        }

        public void PrepareAddInvoiceRequest()
        {
            if (GetClientResponse == null)
            {
                GetClient();
            }

            if (GetOrderResponse == null)
            {
                GetOrder();
            }

            var amount = new RandomHelper().NextDecimal(0, int.MaxValue) + 0.1M;

            AddInvoiceRequest = new AddInvoiceRequest
            {
                ClientId = GetClientResponse.Result.Id,
                OrderId = GetOrderResponse.Result.Id,
                Description = $"New invoice from order {GetOrderResponse.Result.Id} {DateTime.Now.ToLongDateString()}",
                Amount = GetOrderResponse.Result.Amount
            };

        }
    }
}
