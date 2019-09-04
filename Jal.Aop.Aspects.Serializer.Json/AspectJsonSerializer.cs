﻿using Jal.Aop.Aspects.Interface;
using Newtonsoft.Json;

namespace Jal.Aop.Aspects.Serializer.Json
{
    public class AspectJsonSerializer : IAspectSerializer
    {
        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}

