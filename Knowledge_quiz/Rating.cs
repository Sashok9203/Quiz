using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KnowledgeQuiz
{
    [KnownType(typeof(UserQuizInfo))]
    [KnownType(typeof(SortedList<UserQuizInfo, string>))]
    [KnownType(typeof(Dictionary<string, SortedList<UserQuizInfo, string>>))]
    [Serializable]
    internal class Rating :ISerializable
    {
        private Dictionary<string,SortedList<UserQuizInfo,string>> ratingList;

        public Rating() => ratingList = new();

        public void AddQuizInfo(UserQuizInfo? info)
        {
            if (!ratingList.ContainsKey(info?.QuizName ?? "")) ratingList.Add(info?.QuizName, new());
            ratingList[info?.QuizName].Add(info,info.UserName);
        }

        public IEnumerable<UserQuizInfo>? GetQuizInfos(string? quizName)
        {
            IEnumerable<UserQuizInfo>? info = null;
            if (ratingList.ContainsKey(quizName)) info = ratingList[quizName].Keys;
            return info;
        }

        public int GetUserPlace(string? quizName, string? userName)
        {
            if (ratingList.ContainsKey(quizName) && ratingList[quizName].ContainsValue(userName))
               return ratingList[quizName].IndexOfValue(userName) + 1;
            return 0;
        }

        public UserQuizInfo? GetUserQuizInfo(string? quizName,string?userName)
        {
            UserQuizInfo? info = null;
            if (ratingList.ContainsKey(quizName) && ratingList[quizName].ContainsValue(userName))
                info = ratingList[quizName].GetKeyAtIndex(ratingList[quizName].IndexOfValue(userName));
            return info;
        }

        public Rating(SerializationInfo info, StreamingContext context)
        {
            ratingList = info.GetValue("ratingList", typeof(Dictionary<string, SortedList<UserQuizInfo, string>>)) as Dictionary<string, SortedList<UserQuizInfo, string>> ?? new();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ratingList", ratingList);

        }
    }
}
