
using System.Collections;
using System.Runtime.CompilerServices;
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



        public string? GetQuizeQuestionsPath(string quizeName)
        {
            quizzes.TryGetValue(quizeName, out string? path);
            return path;
        }

        public int Count => quizzes?.Count ?? 0;

        public void Clear() => quizzes?.Clear();

        public bool SetQuizesQuestions(string quizeName,string path)
        {
            if(quizzes.ContainsKey(quizeName))
            {
                quizzes[quizeName] = path;
                return true;
            }
            return false;
        }

        public bool DellQuiz(string? quizeName) => quizzes.Remove(quizeName ?? "");

        public void ADDQuiz(string? quizeName, string? path) => quizzes.Remove(quizeName ?? "");

        public void GetObjectData(SerializationInfo info, StreamingContext context) => info.AddValue("Quiz", quizzes);

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => quizzes.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => quizzes.GetEnumerator();
        
    }
}
