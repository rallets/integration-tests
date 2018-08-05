using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stellar.IntegrationTests.Interfaces;
using Stellar.IntegrationTests.RestClient;
using Stellar.IntegrationTests.Core.Interfaces;

namespace Stellar.IntegrationTests.TestApi
{
    public abstract class TestApi<TRequest, TResponse> where TResponse : class 
    {
        protected ILogger _logger;
        protected ITestApiSettings _testSettings;
        public IRestClient RestClient;
        //protected string _name;
        //protected string _url;
        protected Dictionary<string, string> _headers;

        public IRestRequestContext<TRequest> Request;
        public IRestResponseContext<IRestClientResponse<TResponse>> Response;

        public TestApi(ILogger logger)
        {
            _logger = logger;

            RestClient = new Stellar.IntegrationTests.RestClient.RestClient
            {
                OnTrace = logger.Trace
            };

            Request = new RestRequestContext<TRequest>(logger);
            Response = new RestResponseContext<IRestClientResponse<TResponse>>();
        }

        public void Init(ITestApiSettings settings)
        {
            _testSettings = settings;
        }

        public TestApi<TRequest, TResponse> TimeoutRequest(int timeout)
        {
            _testSettings.Timeout = timeout;
            return this;
        }

        public TestApi<TRequest, TResponse> SetRequest(TRequest request)
        {
            Request.Data = request;
            return this;
        }

        public TestApi<TRequest, TResponse> Execute(Func<string, TRequest, Dictionary<string, string>, Task<IRestClientResponse<TResponse>>> restMethod, string url, Dictionary<string, string> headers = null)
        {
            _logger.Write($"{nameof(Execute)} => {restMethod.Method.Name}");

            Response.Data = restMethod(url, Request.Data, headers).Result;

            return this;
        }

        public TestApi<TRequest, TResponse> ValidateResponse(Action<IRestResponseContext<IRestClientResponse<TResponse>>> method)
        {
            ValidateResponse();

            method(Response);

            return this;
        }

        public TestApi<TRequest, TResponse> ValidateResponse(Action<IRestRequestContext<TRequest>, IRestResponseContext<IRestClientResponse<TResponse>>> method)
        {
            ValidateResponse();

            method(Request, Response);

            return this;
        }

        private void ValidateResponse()
        {
            _logger.Write($"Execution time ms: {Response.Data.ExecutionTime.TotalMilliseconds}/{_testSettings.Timeout}");
            Assert.IsTrue(Response.Data.ExecutionTime < TimeSpan.FromMilliseconds(_testSettings.Timeout), $"Execution time {Response.Data.ExecutionTime} exceeded timeout {_testSettings.Timeout}");
        }
        
    }
}
