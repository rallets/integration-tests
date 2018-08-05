using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Stellar.IntegrationTests.Interfaces;
using Stellar.RestApi.Tests.Scenarios.Default;
using Stellar.IntegrationTests.TestApi;
using Stellar.RestApi.Example.Models;

namespace Stellar.RestApi.Tests.Invoices
{
    [TestClass]
    public class EditInvoice : BaseTestApi
    {
        [TestMethod]
        public void Invoices_edit_invoice_success()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.PrepareEditInvoiceRequest();

            var api = new StellarTestApi<EditInvoiceRequest, EditInvoiceResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/invoices/{scenario.EditInvoiceRequest.Id}";

            api
                .SetRequest(scenario.EditInvoiceRequest)
                .Execute(api.RestClient.PutAsync<EditInvoiceResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsValidResponse);
        }

        [TestMethod]
        public void Invoices_edit_invoice_BadRequest_invalid_amount_zero()
        {
            var scenario = new DefaultScenario(_logger);
            scenario.PrepareEditInvoiceRequest();
            scenario.EditInvoiceRequest.Amount = 0;

            var api = new StellarTestApi<EditInvoiceRequest, EditInvoiceResponse>(_logger);

            var url = $"{scenario.BaseUrl}v1/invoices/{scenario.EditInvoiceRequest.Id}";

            api
                .SetRequest(scenario.EditInvoiceRequest)
                .Execute(api.RestClient.PutAsync<EditInvoiceResponse>, url, scenario.DefaultHeaders)
                .ValidateResponse(IsBadRequest);
        }

        public void IsValidResponse(IRestRequestContext<EditInvoiceRequest> request, IRestResponseContext<IRestClientResponse<EditInvoiceResponse>> response)
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
