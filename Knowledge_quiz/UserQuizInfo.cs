using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    [Serializable]
    public class UserQuizInfo : ISerializable
    {
        private int questionCount;

        private int rACount;

        public int RightAnswerCount
        {
            get => rACount; 
            set 
            {
                if (value < 0 || value > questionCount) throw new ApplicationException(" Не вірне значення  кількості правильних відповідей в UserQuizInfo");
                rACount = value;
            }
        }

        public int QuestionCount
        {
            get => questionCount;
            set 
            {
                if(questionCount <= 0) throw new ApplicationException(" Не вірне значення максимальної кількості питань в UserQuizInfo");
                questionCount = value;
            }
        }

        public DateTime Date { get; }
       
        public UserQuizInfo(int qCount,int rACount)
        {
            QuestionCount = qCount;
            RightAnswerCount = rACount;
            Date = DateTime.Now;
        }

        public UserQuizInfo(SerializationInfo info, StreamingContext context)
        {
            Date = info.GetDateTime("Date");
            QuestionCount = info.GetInt32("QuestionCount");
            RightAnswerCount = info.GetInt32("RightAnswerCount");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("QuestionCount", questionCount);
            info.AddValue("RightAnswerCount", rACount);
            info.AddValue("Date", Date);
        }
    }
}
