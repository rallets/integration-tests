using System.Collections.Generic;
using Stellar.IntegrationTests.Core.Helpers;
using Stellar.IntegrationTests.Core.Interfaces;
using Stellar.RestApi.Example.Models;
using Stellar.RestApi.Example.Models.Common;
using Stellar.RestApi.Tests.Scenarios.Default.Clients.Models;
using ECountry = Stellar.RestApi.Example.Models.Common.Country;

namespace Stellar.RestApi.Tests.Scenarios.Default
{
    public partial class DefaultScenario
    {
        private ILogger _logger;

        public TestHelper TestHelper;
        public Dictionary<string, string> DefaultHeaders;

        // shared
        public ECountry? Country;

        // clients
        public GetClientRequest GetClientRequest;
        public GetClientResponse GetClientResponse;

        public GetClientsResponse GetClientsResponse;

        public AddClientRequest AddClientRequest;
        public AddClientResponse AddClientResponse;

        public EditClientRequestExtended EditClientRequest;
        public EditClientResponse EditClientResponse;

        // orders
        public GetOrderRequest GetOrderRequest;
        public GetOrderResponse GetOrderResponse;

        public GetOrdersResponse GetOrdersResponse;

        public AddOrderRequest AddOrderRequest;
        public AddOrderResponse AddOrderResponse;

        public EditOrderRequestExtended EditOrderRequest;
        public EditOrderResponse EditOrderResponse;

        // invoices
        public GetInvoiceRequest GetInvoiceRequest;
        public GetInvoiceResponse GetInvoiceResponse;

        public GetInvoicesResponse GetInvoicesResponse;

        public AddInvoiceRequest AddInvoiceRequest;
        public AddInvoiceResponse AddInvoiceResponse;

        public EditInvoiceRequestExtended EditInvoiceRequest;
        public EditInvoiceResponse EditInvoiceResponse;

        public DefaultScenario(ILogger logger, Country? country = null)
        {
            _logger = logger;
            TestHelper = new TestHelper(_logger);

            if (!country.HasValue)
            {
                country = ECountry.Norway;
            }

            Country = country;

            SetDefaultHeaders();
        }

        public string AuthToken => AppSettingsUtil.Get<string>("Stellar.Example.Authorization.Token");
        public string BaseUrl => AppSettingsUtil.Get<string>("Stellar.Example.BaseUrl");

        private void SetDefaultHeaders()
        {
            DefaultHeaders = new Dictionary<string, string>
            {
                {"Authorization", $"{AuthToken}"},
                {"Accept", "application/json"},
            };
        }

    }
}
