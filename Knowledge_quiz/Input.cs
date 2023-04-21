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

        public static string GetString(string? title, int X, int Y, ConsoleColor titleColor)
        {
            string tmp;
            bool cVisible = Console.CursorVisible;
            if (!cVisible) Console.CursorVisible = true;
            do
            {
                Output.Write(title, X, Y, titleColor);
                tmp = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(tmp));
            Console.CursorVisible = cVisible;
            return tmp ;
        }

        public static string GetWord(string? title, int X, int Y, ConsoleColor titleColor)
        {
            string tmp;
            tmp = GetString(title, X, Y, titleColor);
            tmp = tmp.Trim();
            int ind = tmp.IndexOf(' ');
            return ind < 0 ? tmp : tmp.Substring(0, ind);
        }

        public static string GetPassword(string title,int X, int Y, ConsoleColor titleColor,ConsoleColor passColor)
        {
            ConsoleKeyInfo ch = default;
            bool cVisible = Console.CursorVisible;
            if (!cVisible) Console.CursorVisible = true;
            Output.Write(title, X, Y, titleColor);
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
                        Output.Write("*", passColor);
                    }
                }
            } while (ch.Key != ConsoleKey.Enter || sb.Length == 0 );
            Console.CursorVisible = cVisible;
            return sb.ToString();
        }

        public static int GetInt(int min, int max)
        {
            int value ,X,Y;
            string? str = null;
            X = Console.CursorLeft; 
            Y = Console.CursorTop;
            do
            {
                do
                {
                    string cl = new(' ', str?.Length ?? 0);
                    Console.SetCursorPosition(X, Y);
                    Console.Write(cl);
                    Console.SetCursorPosition(X, Y);
                    str = Input.GetWord(null, X, Y, Console.ForegroundColor);
                }
                while (!int.TryParse(str, out value));
               
            } while (value < min || value > max);
            return value;
        }

        public static DateTime GetDateTime(string? title, int xTitle, int yTitle, int X, int Y, string? yearTitle,string? monthTitle,string? dayTitle, ConsoleColor titleColor, ConsoleColor titlesColor,
            string? hourTitle = null, string? minuteTitle = null, string? secTitle = null ,bool time = false)
        {
            DateTime date = default;
            int year, month, day,
                hour = 0,
                min = 0,
                sec = 0,
                y = Y; 
            Output.Write(title, xTitle, yTitle, titleColor);
            do
            {
                Output.Write(yearTitle, X, y++, titlesColor);
                year = Input.GetInt(1900, 2500);
                Output.Write(monthTitle, X, y++, titlesColor);
                month = Input.GetInt(1, 12);
                Output.Write(dayTitle, X, y++, titlesColor);
                day = Input.GetInt(1, 31);
                if (time)
                {
                    Output.Write(hourTitle, X, y++, titlesColor);
                    hour = Input.GetInt(0, 23);
                    Output.Write(minuteTitle, X, y++, titlesColor);
                    min = Input.GetInt(0, 59);
                    Output.Write(secTitle, X, y++, titlesColor);
                    sec = Input.GetInt(0, 59);
                }
                try
                {
                    try { date = new DateTime(year, month, day, hour, min, sec); }
                    catch { throw new ApplicationException(" Така дата не існує ! ! !"); }
                    if(date > DateTime.Now) throw new ApplicationException(" Не вірна дата народження ! ! !");

                }
                catch(ApplicationException ax)
                {
                    Output.Write(ax.Message, X, y, ConsoleColor.Red);
                    Console.ReadKey();
                    int maxLen = new int[]  { yearTitle?.Length ?? 0, monthTitle?.Length ?? 0, dayTitle?.Length ?? 0, hourTitle?.Length ?? 0, minuteTitle?.Length ?? 0, secTitle?.Length ?? 0 }.Max() + 4;
                    Output.ClearRegion(X, Y,X+maxLen,y);
                    y = Y;
                }
            } while (date == default || date > DateTime.Now);
            return date;
        }
    }
}
