using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Stellar.IntegrationTests.Interfaces;
using Stellar.RestApi.Tests.Scenarios.Default;
using Stellar.IntegrationTests.TestApi;
using ECountry = Stellar.RestApi.Example.Models.Common.Country;
using Stellar.RestApi.Example.Models;
using System.Threading.Tasks;

namespace Stellar.RestApi.Tests.Clients
{
    [TestClass]
    public class GetClient : BaseTestApi
    {
        [TestMethod]
        public void Clients_get_client_success()
        {
            var scenario = new DefaultScenario(_logger, country: ECountry.Norway);
            scenario.PrepareGetClientRequest();

            var api = new StellarTestApi<GetClientRequest, GetClientResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/clients/{scenario.GetClientRequest.Id}";

            api
                .SetRequest(scenario.GetClientRequest)
                .Execute(api.RestClient.GetAsync<GetClientResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsValidResponse);
        }

        [TestMethod]
        public void Clients_get_client_notFound()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.GetClient();

            var api = new StellarTestApi<object, GetClientResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/clients/{Guid.NewGuid()}";

            api
                .SetRequest(null)
                .Execute(api.RestClient.GetAsync<GetClientResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsNotFound);
        }

        [TestMethod]
        public void Clients_get_client_unauthorized()
        {
            var scenario = new DefaultScenario(_logger);

            var api = new StellarTestApi<object, GetClientResponse>(_logger);
            scenario.DefaultHeaders["Authorization"] = "not valid token";

            var url = $"{scenario.BaseUrl}v1/clients/{Guid.NewGuid()}";

            api
                .SetRequest(null)
                .Execute(api.RestClient.GetAsync<GetClientResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsUnauthorized);
        }

        [TestMethod]
        public void Clients_get_client_success_parallel()
        {
            Parallel.For(1, 5, (index) =>
            {
                Clients_get_client_success();
            });
        }

        public void IsValidResponse(IRestRequestContext<GetClientRequest> request, IRestResponseContext<IRestClientResponse<GetClientResponse>> response)
        {
            Assert.AreNotEqual(response.Data, null);
            Assert.AreEqual(response.Data.IsSuccessFul, true);
            Assert.AreEqual(response.Data.Error, null);

            Assert.IsTrue(response.Data.Result.Result.Id != Guid.Empty);
            Assert.IsTrue(response.Data.Result.Result.Name != string.Empty);
        }
    }



}
