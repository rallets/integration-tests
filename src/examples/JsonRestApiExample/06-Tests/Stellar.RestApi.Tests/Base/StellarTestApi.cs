using Stellar.IntegrationTests.Interfaces;
using Stellar.IntegrationTests.Core.Helpers;
using Stellar.IntegrationTests.Core.Interfaces;
using Stellar.IntegrationTests.TestApi;

namespace Stellar.RestApi.Tests
{

    public class StellarTestApiSettings : TestApiSettings
    {
        public StellarTestApiSettings()
        {
            Timeout = AppSettingsUtil.Get<int>("TestApi.Response.Timeout");
        }
    }

    public class StellarTestApi<TRequest, TResponse> : TestApi<TRequest, TResponse>
        where TResponse : class
    {
        public ITestApiSettings _apiSettings;

        public StellarTestApi(ILogger logger, string ContextId = null) : base(logger)
        {
            _logger = logger;
            _apiSettings = new StellarTestApiSettings();

            Init(_apiSettings);
        }

    }
}
