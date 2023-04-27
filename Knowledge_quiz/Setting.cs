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
        private const string defAdminPass   = "admin";

        private const string defQuizzesPath = @"Settings/data.xml";
        private const string defUserPath    = @"Settings/users.xml";
       

        private  string? curentQuizzesPath;
        private  string? curentUserPath;
        
        public string QuizzesPath
        {
            get => Path.Combine(Environment.CurrentDirectory, curentQuizzesPath ?? defQuizzesPath);
            set => curentQuizzesPath = value;
        }

        public string UserPath
        {
            get => Path.Combine(Environment.CurrentDirectory, curentUserPath ?? defUserPath);
            set => curentUserPath = value;
        }

        

        public LPass AdminLogPass { get; private set; }

        public Setting()
        {
            AdminLogPass = new(defAdminLogin, defAdminPass);
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
