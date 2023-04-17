using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    internal class Test
    {
       
        
        public Menu<Test> Menu { get; }
        public Test() 
        {
            Menu = new Menu<Test>("MenuName", 10, 1, ConsoleColor.Red, ConsoleColor.DarkGray, ConsoleColor.Gray
                  , this,  (" Item1",  get ), (" Item2", setget) );
        }

        private void get(Test item)
        {
            Console.Write("Item1");
            Console.ReadKey();

        }
        private void setget(Test item)
        {
            Console.Write("Item2");
            Console.ReadKey();

        }
    }
}
