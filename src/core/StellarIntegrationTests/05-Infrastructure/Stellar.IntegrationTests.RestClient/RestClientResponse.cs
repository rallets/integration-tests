using System;
using System.Net;
using Stellar.IntegrationTests.Interfaces;

namespace Stellar.IntegrationTests.RestClient
{
    public class RestClientResponse<TResult> : IRestClientResponse<TResult> where TResult : class
    {
        public RestClientResponse(TResult result, HttpStatusCode httpStatusCode, TimeSpan executiontime)
        {
            Result = result;
            Error = null;
            ExecutionTime = executiontime;
            HttpStatusCode = (int)httpStatusCode;
        }

        public RestClientResponse(string error, HttpStatusCode httpStatusCode, TimeSpan executiontime)
        {
            Result = null;
            Error = error;
            ExecutionTime = executiontime;
            HttpStatusCode = (int)httpStatusCode;
        }

        public TResult Result { get; private set; }
        public string Error { get; private set; }
        public bool IsSuccessFul => Result != null || (HttpStatusCode >= 200 && HttpStatusCode <= 299);
        public TimeSpan ExecutionTime { get ; private set; }
        public int HttpStatusCode { get; private set; }
    }

}
