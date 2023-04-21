using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    internal partial class Quiz
    {
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
            User curentUser;
            string? login , password;
            Console.Clear();
            Output.WriteLine("-= Вхід в систему =-", x, y++ , ConsoleColor.Magenta);
            login = Input.GetWord("Логін  : ", x, y++, ConsoleColor.Green);
            password = Input.GetPassword("Пароль : ", x, y++, ConsoleColor.Green, ConsoleColor.Green);
            curentUser = users.GetUser(login);
            if (curentUser == null || !curentUser.LoginPass.ChackPassword(password))
            {
                Output.WriteLine("Невірний логін або пароль...", x, y++, ConsoleColor.Magenta);
                Console.ReadKey(true);
                return;
            }
            Output.WriteLine($"Вітаємо в системі {curentUser.Name} ...", x, ++y, ConsoleColor.Green);
            Console.ReadKey(true);
            Console.Clear();
            Menu userMenu = new ($"   -= Меню користувача \"{curentUser?.LoginPass.Login}\" =-", 10, 2, ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
                ("       Топ 20", delegate () { } ),
                ("       Мої результати ", delegate () { }),
                ("       Стартувати вікторину", delegate () {  } ),
                ("       Налаштування", delegate () { } ));
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
                login = Input.GetWord("Введіть ваш логін       : ", x, y, ConsoleColor.Green);
            } while (users.Logins.Contains(login));

            password = Input.GetPassword("Введіть пароль          : ", x, ++y, ConsoleColor.Green, ConsoleColor.Green);

            DateTime date = Input.GetDateTime(null,x,y,x,++y,"Веедіть рік народження : ", 
                "Веедіть місяць народження : ","Веедіть день народження : ",ConsoleColor.Green, ConsoleColor.Green);

            users.AddUser(new User(new LPass(login,Utility.GetHash(password)),name,date));

            Output.Write(" Ви усппішно зареєстровані в системі....", x,Console.CursorTop + 1,  ConsoleColor.Blue);

            Console.ReadKey(true);
        }

    }
}
