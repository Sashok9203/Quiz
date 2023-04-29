
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace KnowledgeQuiz
{
    public static class Serializer
    {
        public static void Serialize<T>(string paths, T obj)
        {
            DataContractSerializer serializer = new(typeof(T));
            using (var fs = XmlWriter.Create(paths , new XmlWriterSettings { Encoding = Encoding.UTF8 }))
                serializer.WriteObject(fs, obj);
        }
        public static T? Deserialize<T>(string paths) where T : class
        {
            DataContractSerializer serializer = new(typeof(T));
            using (XmlReader xr = XmlReader.Create(paths))
            {
                T? tmp = serializer.ReadObject(xr) as T;
                return tmp;
            }
        }
    }
}
