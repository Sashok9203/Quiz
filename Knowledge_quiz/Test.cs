using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    internal class Test
    {
       
        
        public Menu Menu { get; }
        public Test() 
        {
            Menu = new Menu("MenuName", 10, 1, ConsoleColor.Red, ConsoleColor.DarkGray, ConsoleColor.Gray,
                (" Item1", () =>  get(this) ), 
                (" Item2", () =>  setget(this)) );
        }

        private void get(Test item)
        {
            Console.Clear();
            Menu menu = new Menu("Menu2", 10, 1, ConsoleColor.Red, ConsoleColor.DarkGray, ConsoleColor.Gray,
                (" Item1", () => { Console.WriteLine("Item1"); Console.ReadLine(); }),
                (" Item2", () => setget(this)));
            menu.Start();

        }
        private void setget(Test item)
        {
            Console.Write("Item2");
            Console.ReadKey();

        }
    }
}
