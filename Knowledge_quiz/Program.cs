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
            using (Quiz quiz = new())
            {
                quiz.Start();
            }


            //Process proc = new()
            //{
            //    StartInfo = new()
            //    {
            //        CreateNoWindow = false,
            //        UseShellExecute = false,
            //        FileName = "EditUtility.exe",
            //        WindowStyle = ProcessWindowStyle.Normal,
            //        Arguments = ""
                    
            //    }
            //};
            //proc.Start();
            //proc.WaitForExit();
            //Console.WriteLine("Main");
        }
    }

    
}