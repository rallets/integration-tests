using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Stellar.IntegrationTests.Interfaces;
using Stellar.RestApi.Tests.Scenarios.Default;
using Stellar.IntegrationTests.TestApi;
using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Clients
{
    [TestClass]
    public class DeleteClient : BaseTestApi
    {
        [TestMethod]
        public void Clients_delete_client_success()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.AddClient();

            var api = new StellarTestApi<object, object>(_logger);

            var url = $"{scenario.BaseUrl}v1/clients/{scenario.AddClientResponse.Result.Id}";

            api
                .SetRequest(null)
                .Execute(api.RestClient.DeleteAsync<object>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsValidResponse);
        }
        
        public void IsValidResponse(IRestRequestContext<object> request, IRestResponseContext<IRestClientResponse<object>> response)
        {
            Assert.AreNotEqual(response.Data, null);
            Assert.AreEqual(response.Data.IsSuccessFul, true);
            Assert.AreEqual(response.Data.Error, null);

            Assert.IsTrue(response.Data.Result == null);
        }
    }

}
