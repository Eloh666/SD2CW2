using Newtonsoft.Json;

namespace CourseworkTwoMetro.Utils.JSONUtils
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// the JSON serialises with the settings I have added in the lower case contract resolver
    /// </summary>
    public class MyJsonSerializer
    {
        public static string Serialize(object data)
        {
            var settings = new JsonSerializerSettings {ContractResolver = new JsonCustomContractResolver()};
            return JsonConvert.SerializeObject(data, Formatting.Indented, settings);
        }
    }
}