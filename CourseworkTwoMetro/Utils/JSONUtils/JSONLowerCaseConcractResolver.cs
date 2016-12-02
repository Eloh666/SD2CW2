using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CourseworkTwoMetro.Models.Extras;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CourseworkTwoMetro.Utils.JSONUtils
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// a small/personal extention of the json serialize resolver
    /// </summary>
    public class JsonCustomContractResolver : DefaultContractResolver
    {
        // lowercases the json of the serialized object
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }

        // skips the following properties
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