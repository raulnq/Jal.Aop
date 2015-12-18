using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Serializer
{
    public class AspectDataContractSerializer : IAspectSerializer
    {
        public string Serialize(object value, int position)
        {
            using (var ms = new MemoryStream())
            {
                var typeToSerialize = value.GetType();
                var ser = new DataContractSerializer(typeToSerialize);
                ser.WriteObject(ms, value);
                var array = ms.ToArray();
                ms.Close();
                var serializedXml = Encoding.UTF8.GetString(array, 0, array.Length);
                return serializedXml;
            }
        }
    }
}
