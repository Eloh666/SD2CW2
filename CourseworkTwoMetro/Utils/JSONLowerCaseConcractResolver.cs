using Newtonsoft.Json.Serialization;

namespace CourseworkOneMetro.ViewModels.Utils
{
    public class JSONLowerCaseConcractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}