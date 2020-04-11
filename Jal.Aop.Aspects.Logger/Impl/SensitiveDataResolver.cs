using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jal.Aop.Aspects.Logger
{
    public class SensitiveDataResolver<TAttribute> : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            var info = member as PropertyInfo;

            if (info != null)
            {
                var prop = info;
                var isSensitiveData = Attribute.IsDefined(prop, typeof(TAttribute));

                if (isSensitiveData)
                    property.ValueProvider = new SensitiveDataValueProvider("******");
            }

            return property;
        }
    }
}