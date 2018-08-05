using Stellar.IntegrationTests.Core.Helpers;
using Stellar.RestApi.Example.Models;
using System;

namespace Stellar.RestApi.Tests.Scenarios.Default
{
    public partial class DefaultScenario
    {
        public void AddOrder()
        {
            if (AddOrderRequest == null)
            {
                PrepareAddOrderRequest();
            }

            var api = new StellarTestApi<AddOrderRequest, AddOrderResponse>(_logger);

            var url = $"{BaseUrl}v1/orders/";

            api
                .SetRequest(AddOrderRequest)
                .Execute(api.RestClient.PostAsync<AddOrderResponse>, url, DefaultHeaders);

            AddOrderResponse = api.Response.Data.Result;
        }

        public void PrepareAddOrderRequest()
        {
            if (GetClientResponse == null)
            {
                GetClient();
            }

            var amount = new RandomHelper().NextDecimal(0, int.MaxValue) + 0.1M;

            AddOrderRequest = new AddOrderRequest
            {
                ClientId = GetClientResponse.Result.Id,
                Description = $"New order for client {GetClientResponse.Result.Id} {DateTime.Now.ToLongDateString()}",
                Amount = amount
            };

        }
    }
}
