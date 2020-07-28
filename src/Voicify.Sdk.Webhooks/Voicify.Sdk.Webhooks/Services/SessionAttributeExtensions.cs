using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voicify.Sdk.Webhooks.Services
{
    public static class SessionAttributeExtensions
    {
        public static T GetFromJson<T>(this Dictionary<string, object> attributes, string key) where T : class
        {
            try
            {
                if (!attributes.ContainsKey(key)) return null;
                var obj = attributes[key];
                var value = JsonConvert.DeserializeObject<T>(obj.ToString());
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
