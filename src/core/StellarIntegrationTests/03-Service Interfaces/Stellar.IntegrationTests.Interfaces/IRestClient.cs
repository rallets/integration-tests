using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stellar.IntegrationTests.Interfaces
{
    public interface IRestClient : IDisposable
    {
        Task<IRestClientResponse<TReturn>> PostAsync<TReturn>(string url, object data, Dictionary<string, string> headers = null) where TReturn : class;
        Task<IRestClientResponse<TReturn>> PostFormAsync<TReturn>(string url, object data, Dictionary<string, string> headers = null) where TReturn : class;
        Task<IRestClientResponse<TReturn>> PutAsync<TReturn>(string url, object data, Dictionary<string, string> headers = null) where TReturn : class;
        Task<IRestClientResponse<TReturn>> GetAsync<TReturn>(string url, object data, Dictionary<string, string> headers = null) where TReturn : class;
        Task<IRestClientResponse<TReturn>> DeleteAsync<TReturn>(string url, Dictionary<string, string> headers = null) where TReturn : class;
	    Task<IRestClientResponse<TReturn>> DeleteAsync<TReturn>(string url, object data, Dictionary<string, string> headers = null) where TReturn : class;

        Action<string> OnTrace { set; }

        bool CamelCaseRequest { get; set; }
        bool CamelCaseResponse { get; set; }
    }

}
