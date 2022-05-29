using System;
using System.ComponentModel;

namespace Cisco.Otel.Distribution.Tracing
{
    public class Utils
    {
        public static T ReadFromEnv<T>(string variable, T defaultValue)
        {
            return Convert(Environment.GetEnvironmentVariable(variable), defaultValue);
        }

        public static T Convert<T>(string variable, T defaultValue)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    return (T)converter.ConvertFromString(variable);
                }
                return defaultValue;
            }
            catch (NotSupportedException)
            {
                return defaultValue;
            }
        }
    }
}

