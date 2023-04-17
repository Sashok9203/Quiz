using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    
    internal static class Output
    {
        public static void Write(string text, int x , int y, ConsoleColor fColor = default, ConsoleColor bColor = default)
        {
            ConsoleColor bdef = default, fdef = default;
            Console.SetCursorPosition(x, y);
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
          
            Console.Write(text);
            if (bColor != 0) Console.BackgroundColor = bdef;
            if (fColor != 0) Console.ForegroundColor = fdef;
        }
       
        public static void Write(string text, ConsoleColor fColor = default)
        {
            ConsoleColor fdef  = default;
            Console.ForegroundColor = fColor;
            if (fColor != 0)
            {
                fdef = Console.ForegroundColor;
                Console.ForegroundColor = fColor;
            }
            Console.Write(text);
            if (fColor != 0) Console.ForegroundColor = fdef;
        }

        public static void WriteLine(string text, int x, int y, ConsoleColor fColor = default, ConsoleColor bColor = default)
        {
            Write(text + '\n', x, y, fColor, bColor);
        }
        public static void WriteLine(string text, ConsoleColor fColor = default)
        {
            Write(text + '\n', fColor);
        }
        public static void ClearRegion(int Xpos, int YPos, int XCout, int yCount)
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
