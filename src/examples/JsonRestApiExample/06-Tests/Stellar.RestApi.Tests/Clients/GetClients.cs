using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stellar.IntegrationTests.Interfaces;
using Stellar.RestApi.Tests.Scenarios.Default;
using Stellar.IntegrationTests.TestApi;
using ECountry = Stellar.RestApi.Example.Models.Common.Country;
using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Clients
{
    [TestClass]
    public class GetClients : BaseTestApi
    {
        [TestMethod]
        public void Clients_get_all_clients_success()
        {
            var scenario = new DefaultScenario(_logger, country: ECountry.Norway);
            scenario.AddClient();

            var api = new StellarTestApi<object, GetClientsResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/clients/";

            api
                .SetRequest(scenario.GetClientRequest)
                .Execute(api.RestClient.GetAsync<GetClientsResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsValidResponse);
        }

        public void IsValidResponse(IRestRequestContext<object> request, IRestResponseContext<IRestClientResponse<GetClientsResponse>> response)
        {
            Assert.AreNotEqual(response.Data, null);
            Assert.AreEqual(response.Data.IsSuccessFul, true);
            Assert.AreEqual(response.Data.Error, null);

            Assert.IsTrue(response.Data.Result.Result.Count > 0);
        }
    }

}
