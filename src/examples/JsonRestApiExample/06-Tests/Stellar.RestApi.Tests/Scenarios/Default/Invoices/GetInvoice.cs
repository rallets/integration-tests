using System.Linq;
using Stellar.IntegrationTests.Core.Extensions;
using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Scenarios.Default
{
    public partial class DefaultScenario
    {
        public void GetInvoice()
        {
            if (GetInvoiceRequest == null)
            {
                PrepareGetInvoiceRequest();
            }

            var api = new StellarTestApi<GetInvoiceRequest, GetInvoiceResponse>(_logger);

            var url = $"{BaseUrl}v1/invoices/{GetInvoiceRequest.Id}";

            api
                .SetRequest(null)
                .Execute(api.RestClient.GetAsync<GetInvoiceResponse>, url, DefaultHeaders);

            GetInvoiceResponse = api.Response.Data.Result;
        }

        public void PrepareGetInvoiceRequest()
        {
            if (GetInvoicesResponse == null)
            {
                GetInvoices();
            }

            var invoice = GetInvoicesResponse.Result.Randomize().First();

            GetInvoiceRequest = new GetInvoiceRequest
            {
                Id = invoice.Id,
            };

        }
    }
}
