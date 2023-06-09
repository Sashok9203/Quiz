﻿using System.Text;
using System.Text.RegularExpressions;

namespace KnowledgeQuiz
{
    public static class Input
    {
        /// <summary>
        /// Meтод надає інтерфейс користувачу для підтвердження або відміни дії
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Ok"></param>
        /// <param name="Cancel"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="desColor"></param>
        /// <param name="defColor"></param>
        /// <returns></returns>
        public static bool Confirm(string title, string Ok, string Cancel, int X, int Y, ConsoleColor desColor, ConsoleColor defColor)
        {
            bool comfirmed = false;
            ConsoleKey ck = default;
            Output.Write(title, X, Y, defColor);
            Output.Write(Ok, X, Y + 1, desColor);
            Output.Write(" / ", defColor);
            Output.Write(Cancel, defColor);
            do
            {
                if (Console.KeyAvailable)
                {
                    ck = Console.ReadKey(true).Key;
                    if (ck == ConsoleKey.LeftArrow && !comfirmed || ck == ConsoleKey.RightArrow && comfirmed)
                    {
                        comfirmed = !comfirmed;
                        Output.Write(Ok, X, Y + 1, comfirmed ? defColor : desColor);
                        Output.Write(" / ", defColor);
                        Output.Write(Cancel, comfirmed ? desColor : defColor);
                    }
                }
            }
            while (ck != ConsoleKey.Enter);
            Output.ClearRegion(X, Y,Util.Max(title.Length, Ok.Length, Cancel.Length),2);
            return comfirmed;
        }

        /// <summary>
        ///Метод зчитує строку до тих пір поки не буде введена строка яка не складається з пробілів або табуляцій
        /// </summary>
        /// <param name="title"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="titleColor"></param>
        /// <returns></returns>
        public static string GetString(string? title, int X, int Y, ConsoleColor titleColor)
        {
            string? tmp;
            bool cVisible = Console.CursorVisible;
            if (!cVisible) Console.CursorVisible = true;
            do
            {
                if(title != null) Output.Write(title, X, Y, titleColor);
                else Console.SetCursorPosition(X, Y);
                tmp = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(tmp));
            Console.CursorVisible = cVisible;
            return tmp;
        }

        /// <summary>
        ///Метод дає змогу вводити багатостроковий текст
        /// </summary>
        /// <param name="title"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="titleColor"></param>
        /// <returns></returns>
        public static string GetText(string? title, int X, int Y, ConsoleColor titleColor ,ConsoleColor textColor)
        {
            StringBuilder sb = new ();
            ConsoleColor defColor = Console.ForegroundColor;
            bool cVisible = Console.CursorVisible,Empty = true;
            if (!cVisible) Console.CursorVisible = true;
            Output.Write(title, X + 10, Y++, titleColor);
            int i = 0;
            Console.ForegroundColor = textColor;
            do
            {
                Console.SetCursorPosition(X, Y++);
                string? tmp = Console.ReadLine();
                if (string.IsNullOrEmpty(tmp)) i++;
                else 
                {
                    i = 0;
                    Empty = false;
                    sb.AppendLine(tmp);
                }
            } while (i != 2 || Empty);
            Console.CursorVisible = cVisible;
            Console.ForegroundColor = defColor;
            return sb.ToString().Trim();
        }

        /// <summary>
        /// Метод зчитує строку ігноруючи пробіли
        /// </summary>
        /// <param name="title"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="titleColor"></param>
        /// <returns></returns>
        public static string GetWord(string? title, int X, int Y, ConsoleColor titleColor)
        {
            string tmp;
            tmp = GetString(title, X, Y, titleColor);
            tmp = tmp.Trim();
            int ind = tmp.IndexOf(' ');
            return ind < 0 ? tmp : tmp.Substring(0, ind);
        }

        /// <summary>
        /// Метод зчитує строку закриваючи ввід зірочкамипри потребі.Вводити можна лише символи які відповідають регулярному виразу regex
        /// </summary>
        /// <param name="title"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="titleColor"></param>
        /// <param name="passColor"></param>
        /// <returns></returns>
        public static string GetStringRegex(string title,string regex, int X, int Y,int sCount, ConsoleColor titleColor, ConsoleColor passColor,char hideChar =(char)0)
        {
            ConsoleKeyInfo ch = default;
            bool cVisible = Console.CursorVisible,hide = hideChar != 0;
            if (!cVisible) Console.CursorVisible = true;
            Output.Write(title, X, Y, titleColor);
            StringBuilder sb = new ();
            do
            {
                if (Console.KeyAvailable)
                {
                    ch = Console.ReadKey(true);
                    if (ch.Key == ConsoleKey.Backspace && sb.Length != 0)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Y);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Y);
                        sb.Remove(sb.Length - 1, 1);
                    }
                    else if (!Char.IsControl(ch.KeyChar) && Regex.IsMatch($"{ch.KeyChar}", regex) && sb.Length!=sCount)
                    {
                        sb.Append(ch.KeyChar);
                        Output.Write($"{(hide ? hideChar : ch.KeyChar)}", passColor);
                    }
                }
            } while (ch.Key != ConsoleKey.Enter || sb.Length == 0);
            Console.CursorVisible = cVisible;
            return sb.ToString();
        }
        /// <summary>
        /// Метод зчитує значення int в межах від min до max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetInt(int min, int max)
        {
            int value, X, Y;
            string? str = null;
            X = Console.CursorLeft;
            Y = Console.CursorTop;
            do
            {
                string cl = new(' ', str?.Length ?? 0);
                Console.SetCursorPosition(X, Y);
                Console.Write(cl);
                Console.SetCursorPosition(X, Y);
                str = Input.GetWord(null, X, Y, Console.ForegroundColor);
            }
            while (!int.TryParse(str, out value) || value < min || value > max);
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="xTitle"></param>
        /// <param name="yTitle"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="yearTitle"></param>
        /// <param name="monthTitle"></param>
        /// <param name="dayTitle"></param>
        /// <param name="titleColor"></param>
        /// <param name="titlesColor"></param>
        /// <param name="hourTitle"></param>
        /// <param name="minuteTitle"></param>
        /// <param name="secTitle"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(string? title, int xTitle, int yTitle, int X, int Y, string? yearTitle, string? monthTitle, string? dayTitle, ConsoleColor titleColor, ConsoleColor titlesColor,
            string? hourTitle = null, string? minuteTitle = null, string? secTitle = null, bool time = false)
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
                    if (date > DateTime.Now) throw new ApplicationException(" Не вірна дата народження ! ! !");

                }
                catch (ApplicationException ax)
                {
                    Output.Write(ax.Message, X, y, ConsoleColor.Red);
                    Console.ReadKey();
                    Output.ClearRegion(X, Y, X + Util.Max(yearTitle?.Length ?? 0, monthTitle?.Length ?? 0, dayTitle?.Length ?? 0, hourTitle?.Length ?? 0, minuteTitle?.Length ?? 0, secTitle?.Length ?? 0) + 4, 2);
                    y = Y;
                }
            } while (date == default || date > DateTime.Now);
            return date;
        }
    }
}
