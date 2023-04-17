using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    
    internal static class Output
    {
        public static ValueTuple<int,int> Write(string text, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(text);
            return (Console.CursorLeft, Console.CursorTop);
        }

        public static ValueTuple<int, int> Write(string text, int x , int y, ConsoleColor fColor = default, ConsoleColor bColor = default)
        {
            ConsoleColor bdef = default, fdef = default;
           
            if (fColor != 0)
            {
                fdef = Console.ForegroundColor;
                Console.ForegroundColor = fColor;
            }
            if (bColor != 0)
            {
                bdef = Console.BackgroundColor;
                Console.BackgroundColor = bColor;
            }
          
            Write(text,x,y);
            if (bColor != 0) Console.BackgroundColor = bdef;
            if (fColor != 0) Console.ForegroundColor = fdef;
            return (Console.CursorLeft, Console.CursorTop);
        }
       
        public static ValueTuple<int, int> Write(string text, ConsoleColor fColor)
        {
            ConsoleColor fdef = Console.ForegroundColor;
            Console.ForegroundColor = fColor;
            Console.Write(text);
            Console.ForegroundColor = fdef;
            return (Console.CursorLeft, Console.CursorTop);
        }

        public static ValueTuple<int, int> WriteLine(string text, int x, int y, ConsoleColor fColor = default, ConsoleColor bColor = default)
        {
            return Write(text + '\n', x, y, fColor, bColor);
        }

        public static ValueTuple<int, int> WriteLine(string text, ConsoleColor fColor = default)
        {
            return Write(text + '\n', fColor);

        }

        public static void  ClearRegion(int Xpos, int YPos, int XCout, int yCount)
        {
            string tmp = new string(' ', XCout);
            for (int i = 0; i != yCount; i++)
            {
                Console.SetCursorPosition(Xpos, YPos + i);
                Console.Write(tmp);
            }
        }
    }
}
