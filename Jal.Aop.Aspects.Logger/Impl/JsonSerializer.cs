using Newtonsoft.Json;

namespace Jal.Aop.Aspects.Logger
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}

