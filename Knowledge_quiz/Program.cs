using System.Runtime.CompilerServices;
using static KnowledgeQuiz.Menu;

namespace KnowledgeQuiz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] strs = { "Item1", "item2" };
            MenuEventHandler[] hendlers = { delegate () { }, delegate () { } };
            Menu menu = new Menu(" Menu", 1, 1, ConsoleColor.Red, ConsoleColor.DarkGray, ConsoleColor.Gray,strs, hendlers);
            menu.Start();
            Input.Confirm
         
        }
    }

    
}