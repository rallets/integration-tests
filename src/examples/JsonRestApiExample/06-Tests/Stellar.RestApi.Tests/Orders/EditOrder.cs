using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Stellar.IntegrationTests.Interfaces;
using Stellar.RestApi.Tests.Scenarios.Default;
using Stellar.IntegrationTests.TestApi;
using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Orders
{
    [TestClass]
    public class EditOrder : BaseTestApi
    {
        [TestMethod]
        public void Orders_edit_order_success()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.PrepareEditOrderRequest();

            var api = new StellarTestApi<EditOrderRequest, EditOrderResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/orders/{scenario.EditOrderRequest.Id}";

            api
                .SetRequest(scenario.EditOrderRequest)
                .Execute(api.RestClient.PutAsync<EditOrderResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsValidResponse);
        }

        [TestMethod]
        public void Orders_edit_order_BadRequest_invalid_amount_zero()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.PrepareEditOrderRequest();
            scenario.EditOrderRequest.Amount = 0;

            var api = new StellarTestApi<EditOrderRequest, EditOrderResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/orders/{scenario.EditOrderRequest.Id}";

            api
                .SetRequest(scenario.EditOrderRequest)
                .Execute(api.RestClient.PutAsync<EditOrderResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsBadRequest);
        }

        public void IsValidResponse(IRestRequestContext<EditOrderRequest> request, IRestResponseContext<IRestClientResponse<EditOrderResponse>> response)
        {
            Assert.AreNotEqual(response.Data, null);
            Assert.AreEqual(response.Data.IsSuccessFul, true);
            Assert.AreEqual(response.Data.Error, null);

            Assert.IsTrue(response.Data.Result.Result.Id != Guid.Empty);
            Assert.IsTrue(response.Data.Result.Result.ClientId != Guid.Empty);
            Assert.IsTrue(response.Data.Result.Result.ClientId == request.Data.ClientId);
            Assert.IsTrue(response.Data.Result.Result.Amount == request.Data.Amount);
            Assert.IsTrue(response.Data.Result.Result.Amount > 0);
            Assert.IsTrue(response.Data.Result.Result.Description != string.Empty);
        }
    }



}
