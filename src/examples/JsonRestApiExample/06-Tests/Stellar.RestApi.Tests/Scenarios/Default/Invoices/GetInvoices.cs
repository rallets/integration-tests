using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Scenarios.Default
{
    public partial class DefaultScenario
    {

        public void GetInvoices()
        {
            var api = new StellarTestApi<object, GetInvoicesResponse>(_logger);

            var url = $"{BaseUrl}v1/invoices/";

            api
                .SetRequest(null)
                .Execute(api.RestClient.GetAsync<GetInvoicesResponse>, url, DefaultHeaders);

            GetInvoicesResponse = api.Response.Data.Result;

            if(GetInvoicesResponse.Result.Count == 0)
            {
                AddInvoice();

                GetInvoicesResponse.Result.Add(AddInvoiceResponse.Result);
            }
        }
        
    }
}
