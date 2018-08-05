using System;

namespace Stellar.IntegrationTests.Interfaces
{
    public interface IRestClientResponse<TResult> where TResult : class
    {
        TResult Result { get; }
        string Error { get; }
        bool IsSuccessFul { get; }
        TimeSpan ExecutionTime { get; }
        int HttpStatusCode { get; }
    }
}
