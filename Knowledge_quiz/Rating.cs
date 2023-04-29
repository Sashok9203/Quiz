
using System.Runtime.Serialization;


namespace KnowledgeQuiz
{
    [KnownType(typeof(UserQuizInfo))]
    [KnownType(typeof(SortedList<UserQuizInfo, string>))]
    [KnownType(typeof(Dictionary<string, SortedList<UserQuizInfo, string>>))]
    [Serializable]
    internal class Rating :ISerializable
    {
        private readonly Dictionary<string,SortedList<UserQuizInfo,string>> ratingList;

        public Rating() => ratingList = new();

        public void AddQuizInfo(UserQuizInfo info)
        {
            if (!ratingList.ContainsKey(info.QuizName)) ratingList.Add(info.QuizName, new());
            else if (ratingList[info.QuizName].ContainsValue(info.UserName))
                ratingList[info.QuizName].RemoveAt(ratingList[info.QuizName].IndexOfValue(info.UserName));
            ratingList[info.QuizName].Add(info,info.UserName);
        }

        public IEnumerable<UserQuizInfo>? GetQuizInfos(string quizName)
        {
            IEnumerable<UserQuizInfo>? info = null;
            if (ratingList.TryGetValue(quizName, out SortedList<UserQuizInfo, string>? value)) info = value.Keys;
            return info;
        }

        public int GetUserPlace(string quizName, string userName)
        {
            if (ratingList.ContainsKey(quizName) && ratingList[quizName].ContainsValue(userName))
               return ratingList[quizName].IndexOfValue(userName) + 1;
            return 0;
        }

        public IEnumerable<UserQuizInfo>? GetUserQuizInfos(string userName)
        {
            List<UserQuizInfo>? infos = null;

            foreach (var item in ratingList)
            {
                UserQuizInfo? info = GetUserQuizInfo(item.Key, userName);
                if (info != null)
                {
                    infos ??= new();
                    infos.Add(info);
                }
            }
            return infos;
        }

        public UserQuizInfo? GetUserQuizInfo(string quizName,string userName)
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
