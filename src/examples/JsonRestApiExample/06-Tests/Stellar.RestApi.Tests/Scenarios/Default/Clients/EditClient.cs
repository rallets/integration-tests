using System;
using Stellar.RestApi.Example.Models;
using Stellar.RestApi.Tests.Scenarios.Default.Clients.Models;

namespace Stellar.RestApi.Tests.Scenarios.Default
{
    public partial class DefaultScenario
    {
        public void EditClient()
        {
            if (EditClientRequest == null)
            {
                PrepareEditClientRequest();
            }

            var api = new StellarTestApi<EditClientRequest, EditClientResponse>(_logger);

            var url = $"{BaseUrl}v1/clients/{EditClientRequest.Id}";

            api
                .SetRequest(EditClientRequest)
                .Execute(api.RestClient.PostAsync<EditClientResponse>, url, DefaultHeaders);

            EditClientResponse = api.Response.Data.Result;
        }

        public void PrepareEditClientRequest()
        {
            if(GetClientResponse == null)
            {
                GetClient();
            }

            EditClientRequest = new EditClientRequestExtended
            {
                Id = GetClientResponse.Result.Id,
                Country = GetClientResponse.Result.Country,
                Name = $"Edit client {DateTime.Now.ToLongDateString()}"
            };

        }
    }
}
