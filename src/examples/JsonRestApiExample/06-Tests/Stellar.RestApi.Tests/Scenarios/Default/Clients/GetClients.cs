using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Scenarios.Default
{
    public partial class DefaultScenario
    {

        public void GetClients()
        {
            var api = new StellarTestApi<object, GetClientsResponse>(_logger);

            var url = $"{BaseUrl}v1/clients/";

            api
                .SetRequest(null)
                .Execute(api.RestClient.GetAsync<GetClientsResponse>, url, DefaultHeaders);

            GetClientsResponse = api.Response.Data.Result;

            if(GetClientsResponse.Result.Count == 0)
            {
                AddClient();

                GetClientsResponse.Result.Add(AddClientResponse.Result);
            }
        }
        
    }
}
