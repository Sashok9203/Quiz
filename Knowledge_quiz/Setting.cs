using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    [KnownType(typeof(LPass))]
    [Serializable]
    public class Setting : ISerializable
    {
        private const string defAdminLogin  = "admin";
        private const string defAdminPassHash  = "1c224b03cd9bc7b6a86d77f5dace40191766c485cd55dc48caf9ac873335d6f";

        private const string defQuizzesPath = "data.xml";
        private const string defUserPath    = "users.xml";
       

        private  string? curentQuizzesPath;
        private  string? curentUserPath;
        
        public string QuizzesPath
        {
            get => curentQuizzesPath ?? defQuizzesPath;
            set => curentQuizzesPath = value;
        }

        public string UserPath
        {
            get => curentUserPath ?? defUserPath;
            set => curentUserPath = value;
        }

        

        public LPass AdminLogPass { get; private set; }

        public Setting()
        {
            AdminLogPass = new(defAdminLogin, defAdminPassHash);
            curentQuizzesPath = null;
            curentUserPath = null;
        }

        public Setting(SerializationInfo info, StreamingContext context)
        {
            AdminLogPass = info.GetValue("AdminPassLog", typeof(LPass)) as LPass;
            curentQuizzesPath = info.GetString("CurentQuizzesPath");
            curentUserPath = info.GetString("CurentUserPath");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AdminPassLog", AdminLogPass);
            info.AddValue("CurentQuizzesPath", curentQuizzesPath);
            info.AddValue("CurentUserPath", curentUserPath);
        }
    }
}
