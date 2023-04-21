using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    internal static class Input
    {
       
        public static bool Confirm(string title ,string Ok,string Cancel,uint X,uint Y, ConsoleColor desColor, ConsoleColor defColor)
        {
            bool comfirmed = false;
            ConsoleKey ck = default;
            Output.WriteLine(title, (int)X, (int)Y, defColor);
            Output.Write(Ok , (int)X, (int)Y + 1, desColor);
            Output.Write(" / ",defColor);
            Output.WriteLine(Cancel,defColor);
            do
            {
                if (Console.KeyAvailable)
                {
                    ck = Console.ReadKey(true).Key;
                    if (ck == ConsoleKey.LeftArrow && !comfirmed || ck == ConsoleKey.RightArrow && comfirmed)
                    {
                        comfirmed = !comfirmed;
                        Output.Write(Ok , (int)X, (int)Y + 1, comfirmed ? defColor : desColor);
                        Output.Write(" / ", defColor);
                        Output.WriteLine(Cancel, comfirmed ? desColor : defColor);
                    }
                }
            }
            while (ck != ConsoleKey.Enter);
            return comfirmed;
        }

        public static string GetString(string title, int X, int Y, ConsoleColor titleColor)
        {
            string tmp;
            do
            {
                Output.Write(title, X, Y, titleColor);
                tmp = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(tmp));
            return tmp;
        }

        public static string GetWord(string title, int X, int Y, ConsoleColor titleColor)
        {
            string tmp = GetString( title, X, Y,titleColor);
            tmp.Trim();
            int ind = tmp.IndexOf(' ');
            return tmp.Substring(0, ind - 1);
        }

        public static string GetPassword(int X, int Y, ConsoleColor color)
        {
            ConsoleKeyInfo ch = default;
            Console.SetCursorPosition(X, Y);
            StringBuilder sb = new StringBuilder();
            do
            {
                if (Console.KeyAvailable)
                {
                    ch = Console.ReadKey(true);
                    if (ch.Key == ConsoleKey.Backspace)
                    {
                        if (sb.Length != 0)
                        {
                            sb.Remove(sb.Length - 1, 1);
                            Console.SetCursorPosition(Console.CursorLeft - 1, Y);
                            Console.Write(" ");
                            Console.SetCursorPosition(Console.CursorLeft - 1, Y);
                        }
                    }
                    else if(!Char.IsWhiteSpace(ch.KeyChar))
                    {
                        sb.Append(ch.KeyChar);
                        Output.Write("*", color);
                    }
                }
            } while (ch.Key != ConsoleKey.Enter || sb.Length == 0 ); 
            return sb.ToString();
        }

    }
}
