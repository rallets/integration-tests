using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;
using Stellar.IntegrationTests.Interfaces;
using Stellar.IntegrationTests.Core;
using Stellar.IntegrationTests.Core.Interfaces;

namespace Stellar.IntegrationTests.TestApi
{
    public class BaseTestApi
    {
        protected ILogger _logger = new Logger();

        private void IsNotSuccessful<TResponse>(IRestResponseContext<IRestClientResponse<TResponse>> response, HttpStatusCode statusCode) where TResponse : class
        {
            _logger.Write($"Validating response: {JsonConvert.SerializeObject(response)}");

            Assert.IsTrue(response.Data != null, $"{nameof(response.Data)} is '{response.Data}'");
            Assert.IsTrue(response.Data.IsSuccessFul == false, $"{nameof(response.Data.IsSuccessFul)} is '{response.Data.IsSuccessFul}'");
            Assert.IsTrue(response.Data.HttpStatusCode == (int)statusCode, $"{nameof(response.Data.HttpStatusCode)} is '{response.Data.HttpStatusCode}'");
            Assert.IsTrue(response.Data.Result == null, $"{nameof(response.Data.Result)} is '{response.Data.Result}'");
        }

        private void IsSuccessful<TResponse>(IRestResponseContext<IRestClientResponse<TResponse>> response, HttpStatusCode statusCode) where TResponse : class
        {
            _logger.Write($"Validating response: {JsonConvert.SerializeObject(response)}");

            Assert.IsTrue(response.Data != null, $"{nameof(response.Data)} is '{response.Data}'");
            Assert.IsTrue(response.Data.IsSuccessFul, $"{nameof(response.Data.IsSuccessFul)} is '{response.Data.IsSuccessFul}'");
            Assert.IsTrue(response.Data.HttpStatusCode == (int)statusCode, $"{nameof(response.Data.HttpStatusCode)} is '{response.Data.HttpStatusCode}'");
            Assert.IsTrue(response.Data.Result != null, $"{nameof(response.Data.Result)} is '{response.Data.Result}'");
        }

        public void IsBadRequest<TResponse>(IRestResponseContext<IRestClientResponse<TResponse>> response) where TResponse : class
        {
            IsNotSuccessful(response, HttpStatusCode.BadRequest);
        }

        public void IsUnauthorized<TResponse>(IRestResponseContext<IRestClientResponse<TResponse>> response) where TResponse : class
        {
            IsNotSuccessful(response, HttpStatusCode.Unauthorized);
        }

        public void IsMethodNotAllowed<TResponse>(IRestResponseContext<IRestClientResponse<TResponse>> response) where TResponse : class
        {
            IsNotSuccessful(response, HttpStatusCode.MethodNotAllowed);
        }

        public void IsNotFound<TResponse>(IRestResponseContext<IRestClientResponse<TResponse>> response) where TResponse : class
        {
            IsNotSuccessful(response, HttpStatusCode.NotFound);
        }

        public void IsNoContent<TResponse>(IRestResponseContext<IRestClientResponse<TResponse>> response) where TResponse : class
        {
            IsSuccessful(response, HttpStatusCode.NoContent);
        }

        public void IsInternalServerError<TResponse>(IRestResponseContext<IRestClientResponse<TResponse>> response) where TResponse : class
        {
            IsNotSuccessful(response, HttpStatusCode.InternalServerError);
        }

    }
}
