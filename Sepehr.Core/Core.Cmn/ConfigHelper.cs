using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public static class ConfigHelper
    {
        public static T GetConfigValue<T>(string key) where T : IConvertible
        {
            T conf;
            string stringKey = "";
            stringKey = System.Configuration.ConfigurationManager.AppSettings[key];
            if (stringKey != null)
            {
                try
                {
                    conf = (T)Convert.ChangeType(System.Configuration.ConfigurationManager.AppSettings[key], typeof(T));
                    return conf;
                }
                catch (Exception)
                {
                    throw new InvalidCastException("This value[" + key + "] Can not be convert.");
                }
            }
            else
            {
                throw new ArgumentNullException("This key[" + key + "] not found in webconfig or appconfig.");
            }
        }

        public static bool TryGetConfigValue<T>(string key, out T value) where T : IConvertible
        {
            string stringKey = "";
            stringKey = System.Configuration.ConfigurationManager.AppSettings[key];
            if (stringKey != null)
            {
                try
                {
                    value = (T)Convert.ChangeType(System.Configuration.ConfigurationManager.AppSettings[key], typeof(T));
                    return true;
                }
                catch (Exception)
                {
                    throw new InvalidCastException("This value[" + key + "] Can not be convert.");
                }
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public static T GetSection<T>(string query)
        {

            return (T)Convert.ChangeType(System.Configuration.ConfigurationManager.GetSection(query), typeof(T));
        }
    }
}
