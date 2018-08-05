using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Stellar.IntegrationTests.Interfaces;
using Stellar.IntegrationTests.Core.Interfaces;

namespace Stellar.IntegrationTests.TestApi
{
    public class RestRequestContext<T> : IRestRequestContext<T>
    {
        private ILogger _logger;
        private T _data;
        private string _json;

        public RestRequestContext(ILogger logger)
        {
            _logger = logger;
        }

        public bool IsRawJson { get; set; }

        public T Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;

                var json = JsonConvert.SerializeObject(Data, Formatting.None, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                Json = json;
            }
        }

        public void MergeJson(string json)
        {
            _logger.Write($"Merging json '{_json}' with '{json}'");

            JObject o1 = JObject.Parse(_json);
            JObject o2 = JObject.Parse(json);

            o1.Merge(o2, new JsonMergeSettings
            {
                // union array values together to avoid duplicates
                MergeArrayHandling = MergeArrayHandling.Union
            });

            string data = o1.ToString(Formatting.None);

            Json = data;

            _logger.Write($"Merged json is '{Json}'");
        }

        public string Json
        {
            get
            {
                return _json;
            }
            set
            {
                IsRawJson = true;
                _json = value;
            }
        }
    }
}
