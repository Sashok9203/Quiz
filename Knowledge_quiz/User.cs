using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    [KnownType(typeof(UserQuizInfo))]
    [Serializable]
    public class User :ISerializable
    {
        private Dictionary<string,UserQuizInfo> quizzesInfo;

        private string name;

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ApplicationException(" Не вірне ім'я користувача");
                name = value;
            }
        }

        public DateTime Date 
        {
            get;
            set;
        }

        public LPass LoginPass { get; }

        public IEnumerable<KeyValuePair<string, UserQuizInfo>> QuizzesInfoFull => quizzesInfo;

        public IEnumerable<string> QuizzesName => quizzesInfo.Keys;

        public IEnumerable<UserQuizInfo> QuizzesInfo => quizzesInfo.Values;

        public void AddQuizInfo(string qName, UserQuizInfo quizInfo)
        {
            if (quizzesInfo.ContainsKey(qName)) quizzesInfo[qName] = quizInfo;
            else quizzesInfo.Add(qName, quizInfo);
        }

        public User(LPass lPass,string? name, DateTime date)
        {
            LoginPass = lPass;
            Name = name;
            Date = date;
            quizzesInfo = new();
        }

        public User(SerializationInfo info, StreamingContext context)
        {
            LoginPass = info.GetValue("LogPass",typeof(LPass)) as LPass;
            quizzesInfo = info.GetValue("QuizzesInfo", typeof(Dictionary<string, UserQuizInfo>)) as Dictionary<string, UserQuizInfo> ;
            Name = info.GetString("name");
            Date = info.GetDateTime("date");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("LogPass", LoginPass);
            info .AddValue("name", Name);   
            info.AddValue ("date", Date);
            info.AddValue("QuizzesInfo", quizzesInfo);
        }
    }
}
