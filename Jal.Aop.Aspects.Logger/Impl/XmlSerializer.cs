using System.IO;
using System.Text;

namespace Jal.Aop.Aspects.Logger
{
    public class XmlSerializer : ISerializer
    {
        public string Serialize(object value)
        {
            using (var memoryStream = new MemoryStream())
            {
                var typeToSerialize = value.GetType();
                var x = new System.Xml.Serialization.XmlSerializer(typeToSerialize);
                x.Serialize(memoryStream, value);
                var array = memoryStream.ToArray();
                memoryStream.Close();
                var xml = Encoding.UTF8.GetString(array, 0, array.Length);
                return xml;
            }
        }
    }
}
