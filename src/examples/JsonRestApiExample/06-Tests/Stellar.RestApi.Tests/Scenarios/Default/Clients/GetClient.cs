using System.Linq;
using Stellar.IntegrationTests.Core.Extensions;
using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Scenarios.Default
{
    public partial class DefaultScenario
    {
        
        public void GetClient()
        {
            if (GetClientRequest == null)
            {
                PrepareGetClientRequest();
            }

            var api = new StellarTestApi<GetClientRequest, GetClientResponse>(_logger);

            var url = $"{BaseUrl}v1/clients/{GetClientRequest.Id}";

            api
                .SetRequest(null)
                .Execute(api.RestClient.GetAsync<GetClientResponse>, url, DefaultHeaders);

            GetClientResponse = api.Response.Data.Result;
        }

        public void PrepareGetClientRequest()
        {
            if (GetClientsResponse == null)
            {
                GetClients();
            }

            var client = GetClientsResponse.Result.Randomize().First();

            GetClientRequest = new GetClientRequest
            {
                Id = client.Id,
            };

        }
    }
}
