using Newtonsoft.Json;

namespace CourseworkOneMetro.ViewModels.Utils
{
    public class LowcaseJSONKeysSerializer
    {
        public static string Serialize(object data)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new JSONLowerCaseConcractResolver();
            return JsonConvert.SerializeObject(data, Formatting.Indented, settings);
        }
    }
}