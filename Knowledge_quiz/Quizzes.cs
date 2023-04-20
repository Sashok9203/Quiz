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
    [KnownType(typeof(SAQuestion))]
    [KnownType(typeof(MAQuestion))]
    [KnownType(typeof(Dictionary<string, List<Question>>))]
    [Serializable]
    public class Quizzes : ISerializable
    {

        private Dictionary<string, List<Question>>? quizzes;

        public Quizzes(SerializationInfo info, StreamingContext context)
        {
            quizzes = info.GetValue("Quiz", typeof(Dictionary<string, List<Question>>)) as Dictionary<string, List<Question>>;
        }

        public Quizzes()
        {
            quizzes = new();
        }
        public IEnumerable<KeyValuePair<string, List<Question>>>? Quezzes => quizzes;

        public IEnumerable<string>? QuezzesNames => quizzes?.Keys;

        public IEnumerable<List<Question>>? QuezzesQuestions => quizzes?.Values;

        public IEnumerable<Question>? GetQuizeQuestions(string? quizeName) => quizzes?.GetValueOrDefault(quizeName ?? "");

        public Question? GetQuestion(string? quizeName, int questionIndex) => quizzes?.GetValueOrDefault(quizeName ?? "")?[questionIndex];

        public void AddQuize(string? quizeName, params Question[]? questions)
        {
            List<Question> tmp = questions == null ? new List<Question>() : new List<Question>(questions);
            quizzes?.Add(quizeName ?? "InvalidName", tmp);
        }

        public void AddQuestion(string? quizeName, Question? question) => quizzes?[quizeName ?? ""]?.Add(question);
        


        public void show()
        {
            foreach (var item in quizzes)
            {
                Console.WriteLine(item.Key);
                foreach (var i in item.Value)
                {
                    Console.WriteLine(i.QuestionText);
                    foreach (var m in i.AnswerVariants)
                    {
                        Console.WriteLine(m);
                    }
                }
            }
            
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Quiz", quizzes);
        }
    }
}
