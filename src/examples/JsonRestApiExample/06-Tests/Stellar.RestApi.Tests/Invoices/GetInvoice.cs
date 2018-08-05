using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Stellar.IntegrationTests.Interfaces;
using Stellar.RestApi.Tests.Scenarios.Default;
using Stellar.IntegrationTests.TestApi;
using Stellar.RestApi.Example.Models;
using System.Threading.Tasks;

namespace Stellar.RestApi.Tests.Invoices
{
    [TestClass]
    public class GetInvoice : BaseTestApi
    {
        [TestMethod]
        public void Invoices_get_invoice_success()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.PrepareGetInvoiceRequest();

            var api = new StellarTestApi<GetInvoiceRequest, GetInvoiceResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/invoices/{scenario.GetInvoiceRequest.Id}";

            api
                .SetRequest(scenario.GetInvoiceRequest)
                .Execute(api.RestClient.GetAsync<GetInvoiceResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsValidResponse);
        }

        [TestMethod]
        public void Invoices_get_invoice_notFound()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.GetInvoice();

            var api = new StellarTestApi<object, GetInvoiceResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/invoices/{Guid.NewGuid()}";

            api
                .SetRequest(null)
                .Execute(api.RestClient.GetAsync<GetInvoiceResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsNotFound);
        }

        [TestMethod]
        public void Invoices_get_invoice_unauthorized()
        {
            var scenario = new DefaultScenario(_logger);

            var api = new StellarTestApi<object, GetInvoiceResponse>(_logger);
            scenario.DefaultHeaders["Authorization"] = "not valid token";

            var url = $"{scenario.BaseUrl}v1/invoices/{Guid.NewGuid()}";

            api
                .SetRequest(null)
                .Execute(api.RestClient.GetAsync<GetInvoiceResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsUnauthorized);
        }

        [TestMethod]
        public void Invoices_get_invoice_success_parallel()
        {
            Parallel.For(1, 5, (index) =>
            {
                Invoices_get_invoice_success();
            });
        }

        public void IsValidResponse(IRestRequestContext<GetInvoiceRequest> request, IRestResponseContext<IRestClientResponse<GetInvoiceResponse>> response)
        {
            Assert.AreNotEqual(response.Data, null);
            Assert.AreEqual(response.Data.IsSuccessFul, true);
            Assert.AreEqual(response.Data.Error, null);

            Assert.IsTrue(response.Data.Result.Result.Id != Guid.Empty);
            Assert.IsTrue(response.Data.Result.Result.ClientId != Guid.Empty);
            Assert.IsTrue(response.Data.Result.Result.OrderId != Guid.Empty);
            Assert.IsTrue(response.Data.Result.Result.Description != string.Empty);
            Assert.IsTrue(response.Data.Result.Result.Amount > 0);
        }
    }



}
