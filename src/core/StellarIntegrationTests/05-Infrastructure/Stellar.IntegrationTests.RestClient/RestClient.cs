using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stellar.IntegrationTests.Interfaces;
using Stellar.IntegrationTests.Core.Extensions;

namespace Stellar.IntegrationTests.RestClient
{
    public class RestClient : IRestClient
    {
        private Action<string> _onTrace;

        private static HttpClient _client = new HttpClient();

        public bool CamelCaseRequest { get; set; }
        public bool CamelCaseResponse { get; set; }

        public Action<string> OnTrace
        {
            set { _onTrace = value; }
        }

        public RestClient()
        {
            CamelCaseRequest = true;
            CamelCaseResponse = true;

	        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors){
                return true;
            };

            if (_client.Timeout != TimeSpan.FromMinutes(5))
            {
                _client.Timeout = TimeSpan.FromMinutes(5);
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task<IRestClientResponse<TReturn>> PostAsync<TReturn>(string url, object data, Dictionary<string, string> headers = null) where TReturn : class
        {
            return await PostAsyncInternal<TReturn>(url, data, HttpMethod.Post,  headers);
        }

        public async Task<IRestClientResponse<TReturn>> PostFormAsync<TReturn>(string url, object data, Dictionary<string, string> headers = null) where TReturn : class
        {
			LogRequest(url, data, HttpMethod.Post, headers);
	        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
	        if (data != null)
	        {
		        requestMessage.Content = data as FormUrlEncodedContent;
			}
			return await SendRequest<TReturn>(requestMessage, headers);
		}

		public async Task<IRestClientResponse<TReturn>> PutAsync<TReturn>(string url, object data,  Dictionary<string, string> headers = null) where TReturn : class
        {
            return await PostAsyncInternal<TReturn>(url, data, HttpMethod.Put, headers);
        }
        
        public async Task<IRestClientResponse<TReturn>> GetAsync<TReturn>(string url, object data, Dictionary<string, string> headers = null) where TReturn : class
        {
            if (data != null)
            {
                url = url.Contains("?") ? url : url + "?";
                url += data.ToQueryString();
            }

            return await GetAsyncInternal<TReturn>(url, HttpMethod.Get, headers);
        }

        public async Task<IRestClientResponse<TReturn>> DeleteAsync<TReturn>(string url, Dictionary<string, string> headers = null) where TReturn : class
        {
            return await DeleteAsync<TReturn>(url, null, headers);
        }

	    public async Task<IRestClientResponse<TReturn>> DeleteAsync<TReturn>(string url, object data, Dictionary<string, string> headers = null) where TReturn : class
	    {
		    if (data != null)
		    {
			    url = url.Contains("?") ? url : url + "?";
			    url += data.ToQueryString();
		    }

		    return await GetAsyncInternal<TReturn>(url, HttpMethod.Delete, headers);
	    }

		private async Task<IRestClientResponse<TReturn>> PostAsyncInternal<TReturn>(string url, object data, HttpMethod method, Dictionary<string, string> headers = null) where TReturn : class
        {
            LogRequest(url, data, method, headers);

            var requestMessage = CreateRequestMessageAsJson(url, data, method);
            return await SendRequest<TReturn>(requestMessage, headers);
        }

        private HttpRequestMessage CreateRequestMessageAsJson(string url, object data, HttpMethod method)
        {
	        var requestMessage = new HttpRequestMessage(method, url);

            if (data != null)
            {
				SetJsonContent(data, requestMessage);
			}

            return requestMessage;
        }

		private void SetJsonContent(object data, HttpRequestMessage requestMessage)
		{
			StringContent content = null;
			var httpContentData = string.Empty;

			if (data is string)
			{
				httpContentData = data as string;
			}
			else
			{
				var serializationSettings = new JsonSerializerSettings();
                if (CamelCaseRequest)
                {
                    serializationSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                }
				httpContentData = JsonConvert.SerializeObject(data, Formatting.None, serializationSettings);
			}

			content = new StringContent(httpContentData, Encoding.UTF8, "application/json");

			if (content != null)
			{
				requestMessage.Content = content;
			}
		}

		private async Task<IRestClientResponse<TReturn>> GetAsyncInternal<TReturn>(string urlWithQueryStrings, HttpMethod method, Dictionary<string, string> headers = null) where TReturn : class
        {
            LogRequest(urlWithQueryStrings, null, method, headers);

            var requestMessage = new HttpRequestMessage(method, urlWithQueryStrings);
            return await SendRequest<TReturn>(requestMessage, headers);
        }

        private async Task<IRestClientResponse<TReturn>> SendRequest<TReturn>(HttpRequestMessage requestMessage, Dictionary<string, string> headers = null) where TReturn : class
        {
            try
            {
             
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        bool added = requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
	                    if (!added)
                        {
                            throw new ArgumentException($"Invalid header value '{header.Key}' => '{header.Value}'");
                        }
                    }
                }

                DateTime started = DateTime.Now;

                var httpResponseMessage = await _client.SendAsync(requestMessage, CancellationToken.None);

                TimeSpan executionTime = DateTime.Now - started;

                var response = await ConvertToReturnType<TReturn>(httpResponseMessage, executionTime);

                LogResponse(requestMessage, response);

                return response;
            }
            catch (Exception ex)
            {
                _onTrace(string.Format("Unable to complete the request"));
                throw ex;
            }
        }

        private async Task<IRestClientResponse<TReturn>> ConvertToReturnType<TReturn>(HttpResponseMessage result, TimeSpan executionTime) where TReturn : class
        {
            RestClientResponse<TReturn> RestClientResponse;

            if (result.IsSuccessStatusCode)
            {
                TReturn resultContent = null;

                if (typeof(TReturn) == typeof(byte[]))
                {
                    byte[] x = await result.Content.ReadAsByteArrayAsync();
                    resultContent = (TReturn)Convert.ChangeType(x, typeof(TReturn));
                }
                else
                {
                    //resultContent = await result.Content.ReadAsAsync<TReturn>();

                    var rawContent = await result.Content.ReadAsStringAsync();

                    var jsonSerializerSettings = new JsonSerializerSettings();

                    if (CamelCaseResponse)
                    {
                        jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    }

                    resultContent = JsonConvert.DeserializeObject<TReturn>(rawContent, jsonSerializerSettings);
                }

                RestClientResponse = new RestClientResponse<TReturn>(resultContent, result.StatusCode, executionTime);
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                RestClientResponse = new RestClientResponse<TReturn>(error, result.StatusCode, executionTime);
            }

            return RestClientResponse;
        }

        private void LogRequest(string url, object data, HttpMethod method, Dictionary<string, string> headers = null)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                _onTrace($"{method.Method.ToUpperInvariant()} => {url} | headers => {string.Join(",", headers.Select(x => $"[{x.Key}:{x.Value}]"))} | Content: {json}");
            }
            catch (Exception ex)
            {
                _onTrace($"Unable to LogRequest: {ex}");
            }
        }

        private void LogResponse<TReturn>(HttpRequestMessage requestMessage, TReturn response)
        {
            try
            {
                var url = requestMessage.RequestUri;
                var method = requestMessage.Method.Method.ToUpperInvariant();

                var json = JsonConvert.SerializeObject(response, Formatting.None, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                _onTrace($"{method} <= {url} | Response => {json}");
            }
            catch (Exception ex)
            {
                _onTrace($"Unable to LogResponse: {ex}");
            }
        }

    }

    
}