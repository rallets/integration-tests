using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Scenarios.Default
{
    public partial class DefaultScenario
    {

        public void AddClient()
        {
            if (AddClientRequest == null)
            {
                PrepareAddClientRequest();
            }

            var api = new StellarTestApi<AddClientRequest, AddClientResponse>(_logger);

            var url = $"{BaseUrl}v1/clients/";

            api
                .SetRequest(AddClientRequest)
                .Execute(api.RestClient.PostAsync<AddClientResponse>, url, DefaultHeaders);

            AddClientResponse = api.Response.Data.Result;
        }

        public void PrepareAddClientRequest()
        {
            AddClientRequest = new AddClientRequest
            {
                Country = this.Country.Value,
                Name = $"New client {System.DateTime.Now.ToLongDateString()}"
            };

        }
    }
}
