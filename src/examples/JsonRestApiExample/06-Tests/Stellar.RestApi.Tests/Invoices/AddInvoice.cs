using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Stellar.IntegrationTests.Interfaces;
using Stellar.RestApi.Tests.Scenarios.Default;
using Stellar.IntegrationTests.TestApi;
using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Invoices
{
    [TestClass]
    public class AddInvoice : BaseTestApi
    {
        [TestMethod]
        public void Invoices_add_invoice_success()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.PrepareAddInvoiceRequest();

            var api = new StellarTestApi<AddInvoiceRequest, AddInvoiceResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/invoices/";

            api
                .SetRequest(scenario.AddInvoiceRequest)
                .Execute(api.RestClient.PostAsync<AddInvoiceResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsValidResponse);
        }

        [TestMethod]
        public void Invoices_add_invoice_BadRequest_invalid_amount_zero()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.PrepareAddInvoiceRequest();
            scenario.AddInvoiceRequest.Amount = 0;

            var api = new StellarTestApi<AddInvoiceRequest, AddInvoiceResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/invoices/";

            api
                .SetRequest(scenario.AddInvoiceRequest)
                .Execute(api.RestClient.PostAsync<AddInvoiceResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsBadRequest);
        }

        public void IsValidResponse(IRestRequestContext<AddInvoiceRequest> request, IRestResponseContext<IRestClientResponse<AddInvoiceResponse>> response)
        {
            Assert.AreNotEqual(response.Data, null);
            Assert.AreEqual(response.Data.IsSuccessFul, true);
            Assert.AreEqual(response.Data.Error, null);

            Assert.IsTrue(response.Data.Result.Result.Id != Guid.Empty);
            Assert.IsTrue(response.Data.Result.Result.ClientId != Guid.Empty);
            Assert.IsTrue(response.Data.Result.Result.ClientId == request.Data.ClientId);
            Assert.IsTrue(response.Data.Result.Result.OrderId != Guid.Empty);
            Assert.IsTrue(response.Data.Result.Result.OrderId == request.Data.OrderId);
            Assert.IsTrue(response.Data.Result.Result.Amount == request.Data.Amount);
            Assert.IsTrue(response.Data.Result.Result.Amount > 0);
            Assert.IsTrue(response.Data.Result.Result.Description != string.Empty);
        }
    }

}
