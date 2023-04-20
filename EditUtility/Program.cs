using System;
using System.Numerics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using KnowledgeQuiz;

namespace EditUtility
{
    [KnownType(typeof(Quizzes))]
   
    internal class Program 
    {
        
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;


            Quizzes test = new();
            test.AddQuize("Біолоія",
                new SAQuestion("Питання з біології 1","відповідь1", "відповідь2", "відповідь3", "відповідь4"),
                new MAQuestion("Питання з біології 2", "відповідь5", "відповідь6", "відповідь7", "відповідь8"));
            Serializer.Serialize("data.xml", test);
            Quizzes test2 = Serializer.Deserialize<Quizzes>("data.xml");


            
            foreach (var item in test2.Quezzes)
            {
                Console.WriteLine(item.Key);
                foreach (var i in item.Value)
                    Console.WriteLine(i.QuestionText);
            }
            
        }

      
    }
}