using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KnowledgeQuiz
{
    public static class Serializer
    {
        public static void Serialize<T>(string paths, T obj)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (var fs = XmlWriter.Create( paths, new XmlWriterSettings { Encoding = Encoding.UTF32 }))
            {
                serializer.WriteObject(fs, obj);    
                fs.Close();
            }
        }
        public static T Deserialize<T>(string paths) where T: class
        {
          
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (XmlReader xr = XmlReader.Create(new FileStream(paths, FileMode.Open,FileAccess.Read)))
            {
                T tmp = serializer.ReadObject(xr) as T;
                xr.Close();
                return tmp;
            }
        }
    }
}
