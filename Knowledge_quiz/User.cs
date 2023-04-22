using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    [KnownType(typeof(UserQuizInfo))]
    [KnownType(typeof(Dictionary<string, UserQuizInfo>))]
    [KnownType(typeof(LPass))]
    [Serializable]
    public class User :ISerializable
    {
        private readonly Dictionary<string,UserQuizInfo> quizzesInfo;

        private DateTime date;

        public string? Name { get; }
        
        public DateTime Date 
        {
            get => date;
            set
            {
                if (value > DateTime.Now) throw new ApplicationException($" Невірна дата народження {value}...");
                date = value;
            }
        }

        public LPass? LoginPass { get; }

        public UserQuizInfo? GetQuizInfo(string quizName) => quizzesInfo.ContainsKey(quizName) ? quizzesInfo[quizName] : null;

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
            if (string.IsNullOrEmpty(name)) throw new ApplicationException(" Не вірне ім'я користувача");
            LoginPass = lPass;
            Name = name;
            Date = date;
            quizzesInfo = new();
        }

        public User(SerializationInfo info, StreamingContext context)
        {
            LoginPass = info.GetValue("LogPass", typeof(LPass)) as LPass;
            quizzesInfo = info.GetValue("QuizzesInfo", typeof(Dictionary<string, UserQuizInfo>)) as Dictionary<string, UserQuizInfo> ?? new Dictionary<string, UserQuizInfo>();
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

        public override string ToString()
        {
            return "\"" + LoginPass?.Login + "\"" + " " + Name + date.ToShortDateString();
        }
    }
}
