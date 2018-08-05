using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Stellar.IntegrationTests.Interfaces;
using Stellar.RestApi.Tests.Scenarios.Default;
using Stellar.IntegrationTests.TestApi;
using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Clients
{
    [TestClass]
    public class EditClient : BaseTestApi
    {
        [TestMethod]
        public void Clients_edit_client_success()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.PrepareEditClientRequest();

            var api = new StellarTestApi<EditClientRequest, EditClientResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/clients/{scenario.EditClientRequest.Id}";

            api
                .SetRequest(scenario.EditClientRequest)
                .Execute(api.RestClient.PutAsync<EditClientResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsValidResponse);
        }

        public void IsValidResponse(IRestRequestContext<EditClientRequest> request, IRestResponseContext<IRestClientResponse<EditClientResponse>> response)
        {
            Assert.AreNotEqual(response.Data, null);
            Assert.AreEqual(response.Data.IsSuccessFul, true);
            Assert.AreEqual(response.Data.Error, null);

            Assert.IsTrue(response.Data.Result.Result.Id != Guid.Empty);
            Assert.IsTrue(response.Data.Result.Result.Name != string.Empty);
        }
    }



}
