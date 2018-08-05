using System.ComponentModel;
using System.Configuration;

namespace Stellar.IntegrationTests.Core.Helpers
{
    public class AppSettingsUtil
    {
        public static T Get<T>(string keyName, T defaultValue = default(T), bool allowNull = false)
        {
            string value = ConfigurationManager.AppSettings[keyName];
            if (value == null)
            {
                if (defaultValue != null || allowNull)
                {
                    return defaultValue;
                }
                throw new ConfigurationErrorsException($"Key {keyName} not found");
            }
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
        }
    }
}
