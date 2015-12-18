using System.IO;
using System.Text;
using System.Xml.Serialization;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Serializer
{
    public class AspectXmlSerializer : IAspectSerializer
    {
        public string Serialize(object value, int position)
        {
            using (var memoryStream = new MemoryStream())
            {
                var typeToSerialize = value.GetType();
                var x = new XmlSerializer(typeToSerialize);
                x.Serialize(memoryStream, value);
                var array = memoryStream.ToArray();
                memoryStream.Close();
                var xml = Encoding.UTF8.GetString(array, 0, array.Length);
                return xml;
            }
        }
    }
}
