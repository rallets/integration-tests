using System.Linq;
using Stellar.IntegrationTests.Core.Extensions;
using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Scenarios.Default
{
    public partial class DefaultScenario
    {
        public void GetOrder()
        {
            if (GetOrderRequest == null)
            {
                PrepareGetOrderRequest();
            }

            var api = new StellarTestApi<GetOrderRequest, GetOrderResponse>(_logger);

            var url = $"{BaseUrl}v1/orders/{GetOrderRequest.Id}";

            api
                .SetRequest(null)
                .Execute(api.RestClient.GetAsync<GetOrderResponse>, url, DefaultHeaders);

            GetOrderResponse = api.Response.Data.Result;
        }

        public void PrepareGetOrderRequest()
        {
            if (GetOrdersResponse == null)
            {
                GetOrders();
            }

            var order = GetOrdersResponse.Result.Randomize().First();

            GetOrderRequest = new GetOrderRequest
            {
                Id = order.Id,
            };

        }
    }
}
