using System;
using Newtonsoft.Json.Serialization;

namespace Jal.Aop.Aspects.Logger
{
    public class SensitiveDataValueProvider : IValueProvider
    {
        private readonly string _value;

        public SensitiveDataValueProvider(string value)
        {
            _value = value;
        }

        public void SetValue(object target, object value)
        {
            throw new NotSupportedException();
        }

        public object GetValue(object target)
        {
            return _value;
        }
    }
}