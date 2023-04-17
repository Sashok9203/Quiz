using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    internal static class Input
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Ok"></param>
        /// <param name="Cancel"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="desColor"></param>
        /// <param name="defColor"></param>
        /// <returns></returns>
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
    }
}
