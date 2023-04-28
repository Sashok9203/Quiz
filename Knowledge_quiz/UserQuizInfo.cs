using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    [KnownType(typeof(TimeOnly))]
    [Serializable]
    public class UserQuizInfo : ISerializable , IComparable<UserQuizInfo>
    {
        private int questionCount;

        private int rACount;

        private long time;

        public int RightAnswerCount
        {
            get => rACount; 
            set 
            {
                if (value < 0 || value > questionCount) throw new ApplicationException(" Не вірне значення  кількості правильних відповідей в UserQuizInfo");
                rACount = value;
            }
        }

        public string QuizName { get; }

        public string UserName { get; }

        public int QuestionCount
        {
            get => questionCount;
            set 
            {
                if(value <= 0) throw new ApplicationException(" Не вірне значення максимальної кількості питань в UserQuizInfo");
                questionCount = value;
            }
        }

        public DateTime Date { get; }

        public TimeOnly Time { get => new (time);}
       
        public UserQuizInfo(string? userName, string? quizName, int qCount,int rACount,long time)
        {
            QuestionCount = qCount;
            RightAnswerCount = rACount;
            Date = DateTime.Now;
            QuizName = quizName ?? "Invalid quizName";
            UserName = userName ?? "Invalid userName";
            this.time = time;
        }

        public UserQuizInfo(SerializationInfo info, StreamingContext context)
        {
            Date = info.GetDateTime("Date");
            QuestionCount = info.GetInt32("QuestionCount");
            RightAnswerCount = info.GetInt32("RightAnswerCount");
            time = info.GetInt64("Time");
            QuizName = info.GetString("QuizName") ?? "Invalid quizName";
            UserName = info.GetString("UserName") ?? "Invalid userName";
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("QuestionCount", questionCount);
            info.AddValue("RightAnswerCount", rACount);
            info.AddValue("Date", Date);
            info.AddValue("Time", time);
            info.AddValue("QuizName", QuizName);
            info.AddValue("UserName", UserName);
        }

        

       


        public override string ToString()
        {
            return $" {UserName}  {QuestionCount}/{RightAnswerCount}  {Time.ToLongTimeString()} ";
        }

        public int CompareTo(UserQuizInfo? other)
        {
            if (rACount > other?.rACount) return -1;
            else if (rACount == other?.rACount && time > other?.time) return -1;
            else if (rACount == other?.rACount && time == other?.time) return 0;
            else return 1;
        }
    }
}
