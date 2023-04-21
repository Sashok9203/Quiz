using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using static KnowledgeQuiz.Menu;

namespace KnowledgeQuiz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding  = Encoding.Unicode;
            Console.CursorVisible = false;
            using (Quiz quiz = new())
            {
                quiz.Start();
            }
        }
    }

    
}