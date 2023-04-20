using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    public static class Serializer
    {
        public static void Serialize<T>(string paths, T obj)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (Stream fs = new FileStream(paths, FileMode.Create))
            {
                serializer.WriteObject(fs, obj);    
            }
        }
        public static T Deserialize<T>(string paths) where T: class
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (Stream fs = new FileStream(paths, FileMode.Open))
            {
               return serializer.ReadObject(fs) as T ;
            }
        }
    }
}
