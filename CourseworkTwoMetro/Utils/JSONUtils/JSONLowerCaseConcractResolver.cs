using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CourseworkTwoMetro.Models.Extras;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CourseworkTwoMetro.Utils.JSONUtils
{
    public class JsonCustomContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize = instance => property.PropertyName != "breakfast" &&
                                                   property.PropertyName != "dinner" &&
                                                   property.PropertyName != "carhire" &&
                                                   property.PropertyName != "getcost" &&
                                                   property.PropertyName != "startdate" &&
                                                   property.PropertyName != "enddate" &&
                                                   ! property.PropertyName.StartsWith("Validate")
                                                   ;
            return property;
        }


    }

}