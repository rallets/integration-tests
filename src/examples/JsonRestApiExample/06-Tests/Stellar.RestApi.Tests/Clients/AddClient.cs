using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Stellar.IntegrationTests.Interfaces;
using Stellar.RestApi.Tests.Scenarios.Default;
using Stellar.IntegrationTests.TestApi;
using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Clients
{
    [TestClass]
    public class AddClient : BaseTestApi
    {
        [TestMethod]
        public void Clients_add_client_success()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.PrepareAddClientRequest();

            var api = new StellarTestApi<AddClientRequest, AddClientResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/clients/";

            api
                .SetRequest(scenario.AddClientRequest)
                .Execute(api.RestClient.PostAsync<AddClientResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsValidResponse);
        }
        
        public void IsValidResponse(IRestRequestContext<AddClientRequest> request, IRestResponseContext<IRestClientResponse<AddClientResponse>> response)
        {
            Assert.AreNotEqual(response.Data, null);
            Assert.AreEqual(response.Data.IsSuccessFul, true);
            Assert.AreEqual(response.Data.Error, null);

            Assert.IsTrue(response.Data.Result.Result.Id != Guid.Empty);
            Assert.IsTrue(response.Data.Result.Result.Name != string.Empty);
        }
    }

}
