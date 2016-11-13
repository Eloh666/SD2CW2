using Newtonsoft.Json;

namespace CourseworkTwoMetro.Utils.JSONUtils
{
    public class LowcaseJsonKeysSerializer
    {
        public static string Serialize(object data)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new JsonLowerCaseConcractResolver();
            return JsonConvert.SerializeObject(data, Formatting.Indented, settings);
        }
    }
}