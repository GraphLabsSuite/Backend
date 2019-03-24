using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace GraphLabs.Backend.Api
{
    public class LowerCamelCaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return propertyName;
            return $"{propertyName.Substring(0, 1).ToLower()}{propertyName.Substring(1)}";
        }
    }
}