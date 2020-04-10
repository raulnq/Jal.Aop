using System.IO;
using System.Text;

namespace Jal.Aop.Aspects
{
    public class DataContractSerializer : ISerializer
    {
        public string Serialize(object value)
        {
            using (var ms = new MemoryStream())
            {
                var typeToSerialize = value.GetType();
                var ser = new System.Runtime.Serialization.DataContractSerializer(typeToSerialize);
                ser.WriteObject(ms, value);
                var array = ms.ToArray();
                ms.Close();
                var serializedXml = Encoding.UTF8.GetString(array, 0, array.Length);
                return serializedXml;
            }
        }
    }
}
