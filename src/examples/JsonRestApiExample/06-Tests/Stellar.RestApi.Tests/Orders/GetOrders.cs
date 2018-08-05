using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stellar.IntegrationTests.Interfaces;
using Stellar.RestApi.Tests.Scenarios.Default;
using Stellar.IntegrationTests.TestApi;
using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Orders
{
    [TestClass]
    public class GetOrders : BaseTestApi
    {
        [TestMethod]
        public void Orders_get_all_orders_success()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.AddOrder();

            var api = new StellarTestApi<object, GetOrdersResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/orders/";

            api
                .SetRequest(scenario.GetOrderRequest)
                .Execute(api.RestClient.GetAsync<GetOrdersResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsValidResponse);
        }

        public void IsValidResponse(IRestRequestContext<object> request, IRestResponseContext<IRestClientResponse<GetOrdersResponse>> response)
        {
            Assert.AreNotEqual(response.Data, null);
            Assert.AreEqual(response.Data.IsSuccessFul, true);
            Assert.AreEqual(response.Data.Error, null);

            Assert.IsTrue(response.Data.Result.Result.Count > 0);
        }
    }

}
