using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Stellar.IntegrationTests.Interfaces;
using Stellar.RestApi.Tests.Scenarios.Default;
using Stellar.IntegrationTests.TestApi;
using Stellar.RestApi.Example.Models;
using System.Threading.Tasks;

namespace Stellar.RestApi.Tests.Orders
{
    [TestClass]
    public class GetOrder : BaseTestApi
    {
        [TestMethod]
        public void Orders_get_order_success()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.PrepareGetOrderRequest();

            var api = new StellarTestApi<GetOrderRequest, GetOrderResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/orders/{scenario.GetOrderRequest.Id}";

            api
                .SetRequest(scenario.GetOrderRequest)
                .Execute(api.RestClient.GetAsync<GetOrderResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsValidResponse);
        }

        [TestMethod]
        public void Orders_get_order_notFound()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.GetOrder();

            var api = new StellarTestApi<object, GetOrderResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/orders/{Guid.NewGuid()}";

            api
                .SetRequest(null)
                .Execute(api.RestClient.GetAsync<GetOrderResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsNotFound);
        }

        [TestMethod]
        public void Orders_get_order_unauthorized()
        {
            var scenario = new DefaultScenario(_logger);

            var api = new StellarTestApi<object, GetOrderResponse>(_logger);
            scenario.DefaultHeaders["Authorization"] = "not valid token";

            var url = $"{scenario.BaseUrl}v1/orders/{Guid.NewGuid()}";

            api
                .SetRequest(null)
                .Execute(api.RestClient.GetAsync<GetOrderResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsUnauthorized);
        }

        [TestMethod]
        public void Orders_get_order_success_parallel()
        {
            Parallel.For(1, 5, (index) =>
            {
                Orders_get_order_success();
            });
        }

        public void IsValidResponse(IRestRequestContext<GetOrderRequest> request, IRestResponseContext<IRestClientResponse<GetOrderResponse>> response)
        {
            Assert.AreNotEqual(response.Data, null);
            Assert.AreEqual(response.Data.IsSuccessFul, true);
            Assert.AreEqual(response.Data.Error, null);

            Assert.IsTrue(response.Data.Result.Result.Id != Guid.Empty);
            Assert.IsTrue(response.Data.Result.Result.ClientId != Guid.Empty);
            Assert.IsTrue(response.Data.Result.Result.Description != string.Empty);
        }
    }



}
