using Stellar.IntegrationTests.Interfaces;

namespace Stellar.IntegrationTests.TestApi
{
    public abstract class TestApiSettings : ITestApiSettings
    {
        public int Timeout { get; set; }
    }
}
