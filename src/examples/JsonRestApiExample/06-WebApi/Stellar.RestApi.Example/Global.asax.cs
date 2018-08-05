using Newtonsoft.Json.Serialization;
using Stellar.RestApi.Example.App_Start;
using System.Web.Http;
using System.Net.Http.Formatting;

namespace Stellar.RestApi.Example
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Bootstrapper.Run();

            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            GlobalConfiguration.Configuration.Formatters.Clear();
            GlobalConfiguration.Configuration.Formatters.Add(jsonFormatter);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
