using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Scenarios.Default
{
    public partial class DefaultScenario
    {

        public void GetOrders()
        {
            var api = new StellarTestApi<object, GetOrdersResponse>(_logger);

            var url = $"{BaseUrl}v1/orders/";

            api
                .SetRequest(null)
                .Execute(api.RestClient.GetAsync<GetOrdersResponse>, url, DefaultHeaders);

            GetOrdersResponse = api.Response.Data.Result;

            if(GetOrdersResponse.Result.Count == 0)
            {
                AddOrder();

                GetOrdersResponse.Result.Add(AddOrderResponse.Result);
            }
        }
        
    }
}
