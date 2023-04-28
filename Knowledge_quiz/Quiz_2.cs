using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace KnowledgeQuiz
{
    internal partial class Quiz
    {
        /// <summary>
        /// Метод виводить клас UserQuizInfo в консоль
        /// </summary>
        /// <param name="quizInfoName"></param>
        /// <param name="info"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="nameColor"></param>
        /// <param name="infoColor"></param>
        private void printUserQuizInfo(UserQuizInfo info, int X, int Y, ConsoleColor nameColor, ConsoleColor infoColor)
        {
            Output.Write($"Вікторина \"{info.QuizName}\"",X + 2,Y++,nameColor);
            Output.Write($"Кількість питань     : ", X , Y++, infoColor);
            Output.Write($"{info.QuestionCount}", X + 23, Y - 1, ConsoleColor.Gray);
            Output.Write($"Кількість відповідей : ", X, Y++, infoColor);
            Output.Write($"{info.RightAnswerCount}", X + 23, Y - 1, info.RightAnswerCount > info.QuestionCount/3*2? ConsoleColor.Green:
                                                             info.RightAnswerCount > info.QuestionCount / 3 ? ConsoleColor.Yellow:ConsoleColor.Red);
            Output.Write($"Час проходженя       : ", X, Y++, infoColor);
            Output.Write($"{info.Time.ToLongTimeString()}", X + 23, Y - 1, ConsoleColor.Gray);
            Output.Write($"Дата проходження     : ", X , Y++, infoColor);
            Output.Write($"{info.Date}", X + 23, Y - 1, ConsoleColor.Gray);
            Output.Write($"Місце в рейтингу     : ", X, Y++, infoColor);
            Output.Write($"{rating.GetUserPlace(info.QuizName,info.UserName)}", X + 23, Y - 1, ConsoleColor.Gray);
        }

       

       


        // startMenu methods
        /// <summary>
        ///   Метод запускає утіліту редагування Вікторини
        /// </summary>
        /// <param name="path"></param>
        private void StartUtility(string path)  
        {
            Process proc = new()
            {
                StartInfo = new()
                {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    FileName = path,
                    WindowStyle = ProcessWindowStyle.Normal,
                    Arguments = ""

                }
            };
            proc.Start();
            proc.WaitForExit();
        }

        /// <summary>
        ///  Метод входу за логіном на паролем
        /// </summary>
        private void Enter() 
        {
            int x = 14, y = 2;
            User? curentUser;
            string? login , password;
            Console.Clear();
            Output.WriteLine("-= Вхід в систему =-", x, y++ , ConsoleColor.Magenta);
            login = Input.GetStringRegex("Логін  : ", loginRegex, x, y++, ConsoleColor.Green, ConsoleColor.Green);
            password = Input.GetStringRegex("Пароль : ", passwordRegex, x, y++, ConsoleColor.Green, ConsoleColor.Green,'*');
            curentUser = users?.GetUser(login);
            if (curentUser == null || (!curentUser?.LoginPass?.ChackPassword(password) ?? false))
            {
                Output.WriteLine("Невірний логін або пароль...", x, y++, ConsoleColor.Magenta);
                Console.ReadKey(true);
                return;
            }
            Output.WriteLine($"Вітаємо в системі {curentUser?.Name} ...", x, ++y, ConsoleColor.Green);
            Console.ReadKey(true);
            Console.Clear();
            Menu userMenu = new ($"   -= Меню користувача \"{curentUser?.LoginPass?.Login}\" =-", 10, 2, ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
                ("       Топ 20", delegate () { Top20(curentUser); return false; } ),
                ("       Мої результати ", delegate () { MyResults(curentUser); return false; } ),
                ("       Стартувати вікторину", delegate () { QuizStart(curentUser); return false; } ),
                ("       Налаштування", delegate () { Setting(curentUser); return false; } ));
            userMenu.Start();
        }

        /// <summary>
        ///   Метод реєстрації нового користувача
        /// </summary>
        private void Registration()
        {
            int x = 14, y = 2;

            string? name, login = null, password;

            Console.Clear();

            Output.WriteLine("-= Реєстрація нового користувача =-", x - 4, y - 2,ConsoleColor.Magenta);

            name = Input.GetWord("Введіть ваше ім'я       : ", x, y++, ConsoleColor.Green);

            name = name + " " + Input.GetWord("Введіть вашу фамілію    : ", x, y++, ConsoleColor.Green);

            do
            {
                if (login != null)
                {
                    Output.Write("       Такий логін вже існує....", x + 26 + login.Length, y, ConsoleColor.Red);
                    Console.ReadKey();
                    Output.Write(new string(' ', login.Length + 32), x + 26, y, ConsoleColor.Red);
                }
                login = Input.GetStringRegex("Введіть ваш логін       : ", loginRegex, x, y, ConsoleColor.Green, ConsoleColor.Green);
            } while (users?.Logins?.Contains(login) ?? false);

            password = Input.GetStringRegex("Введіть пароль          : ",passwordRegex, x, ++y, ConsoleColor.Green, ConsoleColor.Green,'*');

            DateTime date = Input.GetDateTime(null,x,y,x,++y,"Веедіть рік народження : ", 
                "Веедіть місяць народження : ","Веедіть день народження : ",ConsoleColor.Green, ConsoleColor.Green);

            users?.AddUser(new User(new LPass(login,password),name,date));

            Output.Write(" Ви усппішно зареєстровані в системі....", x,Console.CursorTop + 1,  ConsoleColor.Blue);

            Console.ReadKey(true);
        }


        // userMenu methods
        private void Top20(User? user)
        {
            Console.Clear();
            int index = 1;
            foreach (var item in rating.GetQuizInfos("Математика"))
            {
                Console.WriteLine($"{index++} {item}");
            }
            Console.ReadKey();
        }

        private void MyResults(User? user)
        {
            Console.Clear();
            printUserQuizInfo(rating.GetUserQuizInfo("Математика",user.Name), 1, 1, ConsoleColor.White, ConsoleColor.Yellow);
            Console.ReadKey();
        }

        /// <summary>
        /// Метод формує меню вибору вікторини та запускає процес тестування
        /// </summary>
        /// <param name="user"></param>
        private void QuizStart(User? user)
        {
            string quizName = Quizzes.MixedQuizName;
            Test test;
            Console.Clear();

            var qNames = new List<string>();

            for (int i = 0; i < quizzes.QuizesCount; i++)
                 qNames.Add("\t\t" + quizzes.QuezzesNames.ElementAt(i));

            qNames.Add("\t\t" + quizName);

            Menu quizChooseMenu = new($"   -= Оберіть вікторину \"{user?.LoginPass?.Login}\" =-", 10, 2, ConsoleColor.Green, 

            ConsoleColor.DarkGray, ConsoleColor.Gray, qNames);

            int sel = quizChooseMenu.Start();

            if (sel < 0) return;

            quizName = qNames[sel].Trim();

            try { test = quizzes.GetTest(user.Name,quizName); }
            catch (Exception ex)
            {
                Output.Write(ex.Message, 10, 1);
                Console.ReadKey();
                return;
            }
            
           

            UserQuizInfo qi = test.Start();
            rating.AddQuizInfo(qi);
           // user?.AddQuizInfo(quizName ?? "", uqi);

            Console.Clear();

            Output.Write($"-= Ваш результат =-", 15, 1);

            printUserQuizInfo(qi, 12, Console.CursorTop + 1, ConsoleColor.Green, ConsoleColor.Gray);

            Console.ReadKey();
        } 

        /// <summary>
        /// Meтод реалізує пункт меню "Налаштування"
        /// </summary>
        /// <param name="user"></param>
        private void Setting(User? user)
        {
            Menu? userSettingMenu = null;
            Console.Clear();
             userSettingMenu = new($"   -= Налаштування \"{user?.LoginPass?.Login}\" =-", 10, 2, ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
                ("       Змінити логін",  delegate () { ChangeLogin(user); return true; } ),
                ("       Змінити пароль", delegate () { ChangePassword(user); return true; } ),
                ("       Змінити дату",   delegate () { ChangeDate(user); return true; } ));
            userSettingMenu.Start();
        }


        // userSettingMenu methods
        /// <summary>
        /// Метод заміняє логін користувача
        /// </summary>
        /// <param name="user"></param>
        private void ChangeLogin(User? user)
        {
            int x = 14, y = 2;
            string? login = null,password,oldLogin;
            oldLogin = user?.LoginPass?.Login ;
            Console.Clear();
            Output.Write("-= Заміна логіна =-", x , y++, ConsoleColor.Green);
            do
            {
                if (login != null)
                {
                    Output.Write("       Такий логін вже існує....", x + 17 + login.Length, y, ConsoleColor.Red);
                    Console.ReadKey();
                    Output.Write(new string(' ', login.Length + 32), x + 17, y, ConsoleColor.Red);
                }
                login = Input.GetStringRegex("Введіть новий логін  : ", loginRegex, x, y, ConsoleColor.Green,ConsoleColor.Green);
            } while (users?.Logins?.Contains(login) ?? false);
            password = Input.GetStringRegex("Введіть пароль          : ", passwordRegex, x, ++y, ConsoleColor.Green, ConsoleColor.Green, '*');
            if (user?.LoginPass?.ChangeLogin(login, password) ?? false)
            {
                if( users?.DellUser(oldLogin) ?? false)  users.AddUser(user);
                Output.Write("Логін змінено....", x , ++y, ConsoleColor.Green);
            }
            else Output.Write("Невірний пароль ... Логін не змінено....", x, ++y, ConsoleColor.Red);
            Console.ReadKey(true);
        }

        /// <summary>
        /// Метод заміняє пароль користувача
        /// </summary>
        /// <param name="user"></param>
        private void ChangePassword(User? user)
        {
            int x = 14, y = 2;
            string  password, oldPass;
            Console.Clear();
            Output.Write("-= Заміна пароля =-", x, y++, ConsoleColor.Red);
            oldPass = Input.GetStringRegex(     "Введіть пароль       : ", passwordRegex, x, y++, ConsoleColor.Green, ConsoleColor.DarkGreen);
            if (user?.LoginPass?.ChackPassword(oldPass) ?? false)
            {
                password = Input.GetStringRegex("Введіть новий пароль : ", passwordRegex, x, y++, ConsoleColor.Green, ConsoleColor.DarkGreen);
                user.LoginPass.ChangePassword(password, oldPass);
                Output.Write("Пароль  змінено...", x, y++, ConsoleColor.Red);
            }
            else Output.Write("Не вірний пароль ... Пароль не змінено...", x, y++, ConsoleColor.Red);
            Console.ReadKey();
        }

        /// <summary>
        /// Метод заміняє дату народження користувача
        /// </summary>
        /// <param name="user"></param>
        private void ChangeDate(User? user)
        {
            int x = 14, y = 2;
            Console.Clear();
            Output.Write("-= Заміна дати народження =-", x, y++, ConsoleColor.Green);
            if (user != null) user.Date = Input.GetDateTime(null, x, y, x, ++y, "Веедіть рік народження : ",
                "Веедіть місяць народження : ", "Веедіть день народження : ", ConsoleColor.Green, ConsoleColor.DarkGreen);
            Output.Write("Дату  змінено...", x, Console.CursorTop + 1, ConsoleColor.Red);
            Console.ReadKey();
        }


    }
}
