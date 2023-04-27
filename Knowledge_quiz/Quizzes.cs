using KnowledgeQuiz;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KnowledgeQuiz
{
    
    [KnownType(typeof(Dictionary<string, string>))]
    [Serializable]
    public class Quizzes : ISerializable
    {

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

        public IEnumerable<string> QuezzesNames => quizzes?.Keys.ToArray() ?? Array.Empty<string>();

        public IEnumerable<string> QuezzesPathes => quizzes?.Values.ToArray() ?? Array.Empty<string>();

        public IEnumerable<Question>? AllQuestions
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

        public IEnumerable<Question> GetQuizeQuestions(string? quizeName) => Serializer.Deserialize<Question[]>(Path.Combine(Environment.CurrentDirectory, quizzes[quizeName])) ?? Array.Empty<Question>();

        public int QuizesCount => quizzes?.Count ?? 0;

        public void DellQuiz(string? quizeName) => quizzes.Remove(quizeName ?? "");

        public void ADDQuiz(string? quizeName, string? path) => quizzes.Remove(quizeName ?? "");

        public void GetObjectData(SerializationInfo info, StreamingContext context) => info.AddValue("Quiz", quizzes);
       
    }
}
