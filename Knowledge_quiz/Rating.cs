
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace KnowledgeQuiz
{
    [KnownType(typeof(UserQuizInfo))]
    [KnownType(typeof(SortedList<UserQuizInfo, string>))]
    [KnownType(typeof(Dictionary<string, SortedList<UserQuizInfo, string>>))]
    [Serializable]
    public sealed class Rating :ISerializable
    {
        private readonly Dictionary<string, SortedList<UserQuizInfo, string>> ratingList;

        public Rating() => ratingList = new();

        public bool IsEmpty => ratingList.Count == 0;

        public void AddQuizInfo(UserQuizInfo info)
        {
            if (!ratingList.ContainsKey(info.QuizName))
                ratingList.Add(info.QuizName, new());
            else if (ratingList[info.QuizName].ContainsValue(info.UserName))
                ratingList[info.QuizName].RemoveAt(ratingList[info.QuizName].IndexOfValue(info.UserName));
            ratingList[info.QuizName].Add(info,info.UserName);
        }

        public int  DelUserRating(string userName)
        {
            int count = 0;
            foreach (var item in ratingList)
                if(DelUserQuizRating(item.Key, userName)) count++;
            return count;
        }

        public bool DelUserQuizRating(string quizName, string userName)
        {
            if (ratingList.TryGetValue(quizName, out SortedList<UserQuizInfo, string>? value) && value.ContainsValue(userName))
            {
                value.RemoveAt(ratingList[quizName].IndexOfValue(userName));
                return true;
            }
            return false;
        }

        public bool DelQuizRating(string quizName) => ratingList.Remove(quizName);

        public IEnumerable<string> GetRatingUserNames() => ratingList.Values.SelectMany(n => n.Values).Distinct();
        
        public void Clear() => ratingList.Clear();

        public int GetUserPlace(string quizName, string userName)
        {
            if (ratingList.ContainsKey(quizName) && ratingList[quizName].ContainsValue(userName))
                return ratingList[quizName].IndexOfValue(userName) + 1;
            return 0;
        }

        public IEnumerable<UserQuizInfo>? GetQuizInfos(string quizName)
        {
            ratingList.TryGetValue(quizName, out SortedList<UserQuizInfo, string>? value);
            return value?.Keys;
        }
       
        public IEnumerable<UserQuizInfo> GetUserQuizInfos(string userName) => ratingList.Values.SelectMany(n => n.Keys).Where(n => n.UserName == userName);
        

        public UserQuizInfo? GetUserQuizInfo(string quizName,string userName)
        {
            UserQuizInfo? info = null;
            if (ratingList.TryGetValue(quizName, out SortedList<UserQuizInfo, string>? value) && value.ContainsValue(userName))
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
