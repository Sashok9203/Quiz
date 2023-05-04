
using System.Text;


namespace KnowledgeQuiz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding  = Encoding.Unicode;
            Console.CursorVisible = false;
            Quiz quiz = new();
            quiz.Start();
        }
    }

    
}