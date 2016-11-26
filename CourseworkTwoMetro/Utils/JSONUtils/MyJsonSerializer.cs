using Newtonsoft.Json;

namespace CourseworkTwoMetro.Utils.JSONUtils
{
    public class MyJsonSerializer
    {
        public static string Serialize(object data)
        {
            var settings = new JsonSerializerSettings {ContractResolver = new JsonCustomContractResolver()};
            return JsonConvert.SerializeObject(data, Formatting.Indented, settings);
        }
    }
}