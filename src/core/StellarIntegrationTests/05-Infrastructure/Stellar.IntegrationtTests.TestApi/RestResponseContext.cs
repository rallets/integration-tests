using Stellar.IntegrationTests.Interfaces;

namespace Stellar.IntegrationTests.TestApi
{
    public class RestResponseContext<T> : IRestResponseContext<T>
    {
        public T Data { get; set; }
    }
}
