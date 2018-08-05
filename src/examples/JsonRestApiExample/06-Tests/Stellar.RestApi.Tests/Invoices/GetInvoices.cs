using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stellar.IntegrationTests.Interfaces;
using Stellar.RestApi.Tests.Scenarios.Default;
using Stellar.IntegrationTests.TestApi;
using Stellar.RestApi.Example.Models;
using System;

namespace Stellar.RestApi.Tests.Invoices
{
    [TestClass]
    public class GetInvoices : BaseTestApi
    {
        [TestMethod]
        public void Invoices_get_all_invoices_success()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.AddInvoice();

            var api = new StellarTestApi<object, GetInvoicesResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/invoices/";

            api
                .SetRequest(scenario.GetInvoiceRequest)
                .Execute(api.RestClient.GetAsync<GetInvoicesResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsValidResponse);
        }

        public void IsValidResponse(IRestRequestContext<object> request, IRestResponseContext<IRestClientResponse<GetInvoicesResponse>> response)
        {
            Assert.AreNotEqual(response.Data, null);
            Assert.AreEqual(response.Data.IsSuccessFul, true);
            Assert.AreEqual(response.Data.Error, null);

            Assert.IsTrue(response.Data.Result.Result.Count > 0);

            foreach(var result in response.Data.Result.Result)
            {
                Assert.IsTrue(result.Id != Guid.Empty);
                Assert.IsTrue(result.ClientId != Guid.Empty);
                Assert.IsTrue(result.OrderId != Guid.Empty);
                Assert.IsTrue(result.Description != string.Empty);
                Assert.IsTrue(result.Amount > 0);
            }
        }
    }

}
