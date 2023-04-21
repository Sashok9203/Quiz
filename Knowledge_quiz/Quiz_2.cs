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

        private void Enter() 
        {
            Console.Clear();
            Console.WriteLine(users.UsersInfo.ElementAt(0).Name);
            Console.ReadKey();
        }

        private void Registration()
        {
            int x = 14, y = 2;

            string? name,surname, login = null, password;

            Console.Clear();

            Output.WriteLine("-= Реєстрація нового користувача =-", x - 4, y - 2,ConsoleColor.Magenta);

            name =    Input.GetWord("Введіть ваше ім'я       : ", x, y++, ConsoleColor.Green);

            surname = Input.GetWord("Введіть вашу фамілію    : ", x, y++, ConsoleColor.Green);

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

            DateTime date = Input.GetDateTime(null,x,y,x,++y,"Веедіть рік народження  : ", 
                "Веедіть місяць народження : ","Веедіть день народження : ",ConsoleColor.Green, ConsoleColor.Green);

            users.AddUser(new User(new LPass(login,Utility.GetHash(password)),name,date));

            Output.Write(" Ви усппішно зареєстровані в системі....",  ConsoleColor.Green);

            Console.ReadKey(true);
        }

    }
}
