using System;
using Jal.Aop.Aspects.Interface;
using Newtonsoft.Json;

namespace Jal.Aop.Aspects.Serializer.Json
{
    public class SensitiveDataAspectJsonSerializer<TAttribute> : IAspectSerializer
        where TAttribute : Attribute
    {
        public string Serialize(object value)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new SensitiveDataResolver<TAttribute>() };
            return JsonConvert.SerializeObject(value, settings);
        }
    }
}