using System;
using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Scenarios.Default
{
    public class EditOrderRequestExtended : EditOrderRequest
    {
        public Guid Id { get; set; }
    }

    public partial class DefaultScenario
    {
        public void EditOrder()
        {
            if (EditOrderRequest == null)
            {
                PrepareEditOrderRequest();
            }

            var api = new StellarTestApi<EditOrderRequest, EditOrderResponse>(_logger);

            var url = $"{BaseUrl}v1/orders/{EditOrderRequest.Id}";

            api
                .SetRequest(EditOrderRequest)
                .Execute(api.RestClient.PostAsync<EditOrderResponse>, url, DefaultHeaders);

            EditOrderResponse = api.Response.Data.Result;
        }

        public void PrepareEditOrderRequest()
        {
            if(GetOrderResponse == null)
            {
                GetOrder();
            }

            EditOrderRequest = new EditOrderRequestExtended
            {
                Id = GetOrderResponse.Result.Id,
                ClientId = GetOrderResponse.Result.ClientId,
                Description = $"Edit order {DateTime.Now.ToLongDateString()}",
                Amount = GetOrderResponse.Result.Amount + 1
            };

        }
    }
}
