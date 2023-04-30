
using System.Collections;
using System.Runtime.Serialization;

namespace KnowledgeQuiz
{
    
    [KnownType(typeof(Dictionary<string, string>))]
    [Serializable]
    public class Quizzes : ISerializable,IEnumerable<KeyValuePair<string,string>>
    {
        public const string MixedQuizName = "Змішана";

        private readonly Dictionary<string, string> quizzes;

        public Quizzes(SerializationInfo info, StreamingContext context)
        {
            quizzes = info.GetValue("Quiz", typeof(Dictionary<string, string>)) as Dictionary<string, string> ?? new ();
        }

        public Quizzes()
        {
            quizzes = new()
            {
                //{ "Математика", @"Questions\math.xml" },
                //{ "Біологія", @"Questions\biology.xml" }
            };
        }
       

        public IEnumerable<string> QuezzesNames => quizzes.Keys ;

        public IEnumerable<Question> AllQuestions
        {
            get 
            {
                List<Question> list =  new();
                foreach (var item in quizzes)
                {
                    Question[]? des = Serializer.Deserialize<Question[]>(Path.Combine(Environment.CurrentDirectory, item.Value));
                    if(des != null) list.AddRange(des);
                }
                return list;
            }
        }

        public IEnumerable<Question> GetQuizeQuestions(string quizeName) => Serializer.Deserialize<Question[]>(Path.Combine(Environment.CurrentDirectory, quizzes[quizeName])) ?? Array.Empty<Question>();

        public int Count => quizzes?.Count ?? 0;

        public void Clear() => quizzes?.Clear();

        /// <summary>
        /// Метод повертає клас Test який містить питання в залежності від назви вікторини 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public  Test GetTest(string userName,string quizeName)
        {
            const int maxQustionCountInQuiz = 20;
            IEnumerable<Question> questions;
            if (quizeName != MixedQuizName) questions = Utility.Shufflet(GetQuizeQuestions(quizeName));
            else questions = Utility.Shufflet(AllQuestions);
            if (questions.Count() == 0) throw new ApplicationException($"Вісторина \"{quizeName}\" не містить питань");
            int questionCount = questions.Count() < maxQustionCountInQuiz ? questions.Count() : maxQustionCountInQuiz;
            return new Test(userName,quizeName, questions.Take(questionCount));
        }

        public void DellQuiz(string? quizeName) => quizzes.Remove(quizeName ?? "");

        public void ADDQuiz(string? quizeName, string? path) => quizzes.Remove(quizeName ?? "");

        public void GetObjectData(SerializationInfo info, StreamingContext context) => info.AddValue("Quiz", quizzes);

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => quizzes.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => quizzes.GetEnumerator();
        
    }
}
