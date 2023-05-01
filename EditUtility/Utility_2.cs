using KnowledgeQuiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace EditUtility
{
    internal partial class Utility
    {
        private bool Setting()
        {
            int X = 10, Y = 1;
            Menu settingMenu = new($"   -= Налаштування =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
              ("     Змінити логін", ChangeLogin),
              ("     Змінити пароль", ChangePassword),
              ("     Змінити каталог з інформацією",() => { ChangeDir("Зміна каталогу з інформацією ",true); return false; }),
              ("     Змінити каталог з питаннями",  () => { ChangeDir("Зміна каталогу з питаннями ", false); return false; }  ) )
                {
                    XPos = X,
                    YPos = Y
                };
            
            settingMenu.Start();
            settingMenu.Hide();
            return false;
        }
        private bool ChangePassword()
        {
            int x = 14, y = 2;
            string password, oldPass;
            Console.Clear();
            Output.Write("-= Заміна пароля =-", x, y++, ConsoleColor.Red);
            oldPass = Input.GetStringRegex("Введіть пароль       : ", passwordRegex, x, y++, ConsoleColor.Green, ConsoleColor.DarkGreen);
            if (users.AdminLogPass.ChackPassword(oldPass))
            {
                password = Input.GetStringRegex("Введіть новий пароль : ", passwordRegex, x, y++, ConsoleColor.Green, ConsoleColor.DarkGreen);
                users.AdminLogPass.ChangePassword(password, oldPass);
                Output.Write("Пароль  змінено...", x, y++, ConsoleColor.Red);
                SLSystem.SaveUsers();
            }
            else Output.Write("Не вірний пароль ... Пароль не змінено...", x, y++, ConsoleColor.Red);
            Console.ReadKey();
            Console.Clear();
            return false;
        }
        private bool ChangeLogin()
        {
            int x = 14, y = 2;
            string login, password;
            Console.Clear();
            Output.Write("-= Заміна логіна =-", x, y++, ConsoleColor.Red);
            password = Input.GetStringRegex("Введіть пароль       : ", passwordRegex, x, y++, ConsoleColor.Green, ConsoleColor.DarkGreen);
            if (users.AdminLogPass.ChackPassword(password))
            {
                login = Input.GetStringRegex("Введіть новий логін : ", loginRegex, x, y++, ConsoleColor.Green, ConsoleColor.DarkGreen);
                users.AdminLogPass.ChangeLogin(login, password);
                Output.Write("Логін  змінено...", x, y++, ConsoleColor.Red);
                SLSystem.SaveUsers();
            }
            else Output.Write("Не вірний пароль ... Логін не змінено...", x, y++, ConsoleColor.Red);
            Console.ReadKey();
            Console.Clear();
            return false;
        }
        private void ChangeDir(string title, bool dir)
        {
            int x = 3, y = 1;
            Output.Write($" -= {title} =-",x,y++,ConsoleColor.Green);
            Output.Write($"Поточний шлях каталогу : {(dir?SLSystem.DataDir:SLSystem.QuestionDir)}", x, y++, ConsoleColor.Red);
            string newDir = Input.GetString("Введіть шлях до нового каталогу : ",x,y++,ConsoleColor.Green);
            newDir = newDir.Trim();
            try
            {
                if (!Directory.Exists(newDir)) Directory.CreateDirectory(newDir);
                string[] files = Directory.GetFiles((dir ? SLSystem.DataDir : SLSystem.QuestionDir));
                if (files.Length != 0)
                {
                    if (Input.Confirm("Скопіювати файли зі старої директорії?", "Так", "Ні", x, y, ConsoleColor.DarkGray, ConsoleColor.Green))
                    {
                        foreach (var item in files)
                        {
                            File.Copy(item, Path.Combine(newDir, Path.GetFileName(item)), true);
                            Output.Write($"Зкопійовано : \"{Path.GetFileName(item)}\"", x, y++, ConsoleColor.Green);
                        }
                    }
                }
                if (dir) SLSystem.DataDir = newDir;
                else SLSystem.QuestionDir = newDir;
                Output.Write($"Каталог з  змінено...", x, y++, ConsoleColor.Green);
                SLSystem.SaveSettings();
            }
            catch { Output.Write($"Не вірний шлях  : {newDir}", x, y++, ConsoleColor.Red); }
          
            Console.ReadKey();

            Console.Clear();
        }


        private bool Users()
        {
            int X = 10, Y = 1;
            Menu usersMenu = new("   -= Користувачі =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
              ("Інформація про користувачів", UsersShow),
              ("Видалити користувача", DelUser) )
            {
                XPos = X,
                YPos = Y
            };

            usersMenu.Start();
            usersMenu.Hide();
            return false;
        }
        private bool UsersShow()
        {

            return false;
        }
        private bool DelUser()
        {
            int X = 10, Y = 1;
            int sel = 0;
            Menu menu = new($"   -= Оберіть користувача =-", ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.Gray)
            {
                XPos = X,
                YPos = Y
            };
            do
            {
                Console.Clear();
                menu.Clear();
                int x = X, y = Y;
                if (users.Count != 0)
                {
                    IEnumerable<User> allUsers = users.AllUsers;
                    foreach (var item in allUsers)
                        menu.AddMenuItem(($"   {item}", null));
                    sel = menu.Start();
                    if (sel >= 0)
                    {
                        if (Input.Confirm("Ви дійсно хочете видалити цього користувача?", "Так", "Ні", x, Console.CursorTop + 1,
                             ConsoleColor.DarkGray, ConsoleColor.Green))
                        {
                            users.DellUser(allUsers.ElementAt(sel).LoginPass.Login);
                            Output.Write("Користувача видалено....", x, Console.CursorTop + 1, ConsoleColor.Red);
                            SLSystem.SaveUsers();
                        }
                    }
                    else sel = -1;

                }
                else { Output.Write("Немає зареєстрованих користувачів....", x, y, ConsoleColor.Red); }
                if(sel >= 0) Console.ReadKey(); ;
            } while (users.Count != 0 && sel > 0);
            Console.Clear();
            return false;
        }

    }
}
