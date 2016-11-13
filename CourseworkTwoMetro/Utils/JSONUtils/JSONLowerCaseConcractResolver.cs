using Newtonsoft.Json.Serialization;

namespace CourseworkTwoMetro.Utils.JSONUtils
{
    public class JsonLowerCaseConcractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}