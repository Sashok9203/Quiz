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
        private void showUser(User user)
        {
            int X = 10, Y = 1;
            Output.Write($"  -= Користувач \"логін: < {user.LoginPass.Login} >\" =-", X, Y++, ConsoleColor.Red);
            Output.Write($"Ім'я та прізвище  : {user.Name}", X, Y++, ConsoleColor.Green);
            Output.Write($"Дата народження   : {user.Date.ToLongDateString()}", X, Y++, ConsoleColor.Green);
            Output.Write($"Дата реєстрації   : {user.RegistrationDate.ToLongDateString()}", X, Y++, ConsoleColor.Green);
            Output.Write($"Пройдені віторини : ", X, Y, ConsoleColor.Green);
            IEnumerable<UserQuizInfo> infos = rating.GetUserQuizInfos(user.Name) ?? Array.Empty<UserQuizInfo>();
            if (!infos.Any()) Console.Write($"відсутні...");
            else 
            {
                ConsoleColor color;
                foreach (var item in infos)
                {
                    int tmp = item.QuestionCount / 3;
                    if (item.RightAnswerCount <= tmp) color = ConsoleColor.Red;
                    else if (item.RightAnswerCount > tmp && item.RightAnswerCount < tmp * 2) color = ConsoleColor.Yellow;
                    else color = ConsoleColor.Green;
                    Output.Write($" \"{item.QuizName}\"",color);
                }
            }
            
            Console.ReadKey();
            Console.Clear();
        }
        private void setQuestionFile(string quizName)
        {
            int X = 5, Y = 1;
            int sel;
            string fileName;
            IEnumerable<string> files = Directory.GetFiles(SLSystem.QuestionDir);
            if (files.Any())
            {
                Menu fileChooseMenu = new($"   -= Оберіть файл з питаннями =-", ConsoleColor.Green,
                                          ConsoleColor.DarkGray, ConsoleColor.Gray);
                foreach (var q in files)
                    fileChooseMenu.AddMenuItem(($"         {Path.GetFileName(q)}", null));
                fileChooseMenu.XPos = X + 5;
                fileChooseMenu.YPos = Y + 1;

                sel = fileChooseMenu.Start();
                if (sel >= 0)
                {
                    fileName = files.ElementAt(sel);
                    if (Input.Confirm("Ви впевненні ?", "Так", "Ні", X, Console.CursorTop + 2, ConsoleColor.DarkGray, ConsoleColor.Green))
                    {
                        if (quizzes.SetQuizesQuestions(quizName, fileName))
                        {
                            SLSystem.SaveQuizzes();
                            Output.Write($"Вікторини {quizName} встановлені питання {fileName} ...", X, Console.CursorTop, ConsoleColor.Green);
                        }
                    }
                }
            }
            else Output.Write($"Директорія з питаннями пуста ...", X, Console.CursorTop, ConsoleColor.Red);
            Console.ReadKey();
            Console.Clear();
        }




        private void QuizzesEdit()
        {
            int X = 10, Y = 1;
            Menu settingMenu = new($"   -= Вікторини =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
              ("     Видалити вікторину", DelQuiz),
              ("     Додати вікторину", () => { } ),
              ("     Завантажити питання",  LoadQuestionsFile),
              ("     Створити файл з питаннями", () => {  } ),
              ("     Редагувати файл з питаннями", () => { }))
            {
                XPos = X,
                YPos = Y
            };

            settingMenu.Start();
            settingMenu.Hide();
            
        }
        private void DelQuiz()
        {
            int X = 5, Y = 1;
            int sel;
            string quizName ;
            if (quizzes.Count != 0)
            {
                Menu quizChooseMenu = new($"   -= Оберіть назву вікторини =-", ConsoleColor.Green,
                                      ConsoleColor.DarkGray, ConsoleColor.Gray)
                {
                    XPos = X + 5,
                    YPos = Y + 1
                };

                do
                {

                    foreach (var q in quizzes)
                        quizChooseMenu.AddMenuItem(($"         {q.Key}", null));
                    sel = quizChooseMenu.Start();
                    if (sel >= 0)
                    {
                        quizName = quizzes.QuezzesNames.ElementAt(sel);
                        if (Input.Confirm("Ви впевненні ?", "Так", "Ні", X, Console.CursorTop + 2, ConsoleColor.DarkGray, ConsoleColor.Green))
                        {
                            if (quizzes.DellQuiz(quizName))
                            {
                                if (quizzes.Count == 0) sel = -1;
                                // SLSystem.SaveQuizzes();
                                Output.Write($"Вікторини {quizName} видалена ...", X, Console.CursorTop, ConsoleColor.Green);
                            }
                            Console.ReadKey();
                            Output.Write(new string(' ', 60), X, Console.CursorTop);
                            quizChooseMenu.Hide();
                            quizChooseMenu.Clear();
                        }
                    }

                } while (sel >= 0);
            }
            if (quizzes.Count == 0)
            {
                Output.Write($"Вікторини  відсутні ...", X,Y, ConsoleColor.Green);
                Console.ReadKey();
            }
            
            Console.Clear();
        }
        private void LoadQuestionsFile()
        {
            int X = 5, Y = 1;
            int sel;
            string quizName;
            Menu quizChooseMenu = new($"   -= Оберіть назву вікторини =-", ConsoleColor.Green,
                                      ConsoleColor.DarkGray, ConsoleColor.Gray);
            foreach (var q in quizzes)
                quizChooseMenu.AddMenuItem(($"         {q.Key}", null));
            quizChooseMenu.XPos = X + 5;
            quizChooseMenu.YPos = Y + 1;

            do
            {
                sel = quizChooseMenu.Start();
                if (sel >= 0)
                {
                    quizName = quizzes.QuezzesNames.ElementAt(sel);
                    quizChooseMenu.Hide();
                    setQuestionFile(quizName);
                }

            } while (sel >= 0);
            Console.Clear();
        }



        private void Setting()
        {
            int X = 10, Y = 1;
            Menu settingMenu = new($"   -= Налаштування =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
              ("     Змінити логін", ChangeLogin),
              ("     Змінити пароль", ChangePassword),
              ("     Змінити каталог з інформацією",() =>  ChangeDir("Зміна каталогу з інформацією ",true) ),
              ("     Змінити каталог з питаннями",  () =>  ChangeDir("Зміна каталогу з питаннями ", false)  ) )
                {
                    XPos = X,
                    YPos = Y
                };
            
            settingMenu.Start();
            settingMenu.Hide();
           
        }
        private void ChangePassword()
        {
            int x = 14, y = 2;
            string password, oldPass;
            Console.Clear();
            Output.Write("-= Заміна пароля =-", x, y++, ConsoleColor.Red);
            oldPass = Input.GetStringRegex("Введіть пароль       : ", passwordRegex, x, y++,passwordMaxLenght, ConsoleColor.Green, ConsoleColor.DarkGreen);
            if (users.AdminLogPass.ChackPassword(oldPass))
            {
                password = Input.GetStringRegex("Введіть новий пароль : ", passwordRegex, x, y++,passwordMaxLenght, ConsoleColor.Green, ConsoleColor.DarkGreen);
                users.AdminLogPass.ChangePassword(password, oldPass);
                Output.Write("Пароль  змінено...", x, y++, ConsoleColor.Red);
                SLSystem.SaveUsers();
            }
            else Output.Write("Не вірний пароль ... Пароль не змінено...", x, y++, ConsoleColor.Red);
            Console.ReadKey();
            Console.Clear();
           
        }
        private void ChangeLogin()
        {
            int x = 14, y = 2;
            string login, password;
            Console.Clear();
            Output.Write("-= Заміна логіна =-", x, y++, ConsoleColor.Red);
            password = Input.GetStringRegex("Введіть пароль       : ", passwordRegex, x, y++,passwordMaxLenght, ConsoleColor.Green, ConsoleColor.DarkGreen);
            if (users.AdminLogPass.ChackPassword(password))
            {
                login = Input.GetStringRegex("Введіть новий логін : ", loginRegex, x, y++,loginMaxLenght, ConsoleColor.Green, ConsoleColor.DarkGreen);
                users.AdminLogPass.ChangeLogin(login, password);
                Output.Write("Логін  змінено...", x, y++, ConsoleColor.Red);
                SLSystem.SaveUsers();
            }
            else Output.Write("Не вірний пароль ... Логін не змінено...", x, y++, ConsoleColor.Red);
            Console.ReadKey();
            Console.Clear();
           
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

        private void Users()
        {
            int X = 10, Y = 1;
            Menu usersMenu = new("   -= Користувачі =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
              ("Інформація про користувачів", UsersInfo),
              ("Видалити користувача", DelUser) )
            {
                XPos = X,
                YPos = Y
            };

            usersMenu.Start();
            usersMenu.Hide();
        }
        private void UsersInfo()
        {
            int X = 10, Y = 1;
            int sel = 0;
            Menu menu = new($"---- Логін --------- Ім'я ----", ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.Gray)
            {
                XPos = X,
                YPos = Y + 1
            };
            IEnumerable<User> allUsers = users.AllUsers;
            foreach (var item in allUsers)
                menu.AddMenuItem(($"     {item}", null));
           
            do
            {
                Output.Write("  -= Оберіть користувача =-", X, Y, ConsoleColor.Red);
                sel = menu.Start();
                if (sel >= 0)
                {
                    menu.Hide();
                    showUser(allUsers.ElementAt(sel));
                }
            } while (sel >= 0) ;
                Console.Clear();
        }
        private void DelUser()
        {
            int X = 10, Y = 1;
            int sel = 0;
            Output.Write("  -= Оберіть користувача =-", X, Y, ConsoleColor.Red);
            Menu menu = new($"---- Логін --------- Ім'я ----", ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.Gray)
            {
                XPos = X,
                YPos = Y+1
            };
            do
            {
                //Console.Clear();
                menu.Clear();
                int x = X, y = Y;
                if (users.Count != 0)
                {
                    IEnumerable<User> allUsers = users.AllUsers;
                    foreach (var item in allUsers)
                        menu.AddMenuItem(($"     {item}", null));
                    sel = menu.Start();
                    if (sel >= 0)
                    {
                        if (Input.Confirm("Ви дійсно хочете видалити цього користувача?", "Так", "Ні", x, Console.CursorTop + 1,
                             ConsoleColor.DarkGray, ConsoleColor.Green))
                        {
                            users.DellUser(allUsers.ElementAt(sel).LoginPass.Login);
                            SLSystem.SaveUsers();
                            menu.Hide();
                        }
                    }
                    else sel = -1;
                }
                if (users.Count == 0)
                {
                    Output.Write("Немає зареєстрованих користувачів....", x, y, ConsoleColor.Red);
                    Console.ReadKey();
                }
            } while (users.Count != 0 && sel >= 0);
            Console.Clear();
        }

        private void Ratings()
        {
            int X = 10, Y = 1;
            Menu usersMenu = new("   -= Рейтинги =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
              ("Видалити рейтинги вікторини", DelQuizRating),
              ("Видалити рейтинги користувача", DelUserRatings),
              ("Видалити рейтинги незареєстрованих", DelNotRegUserRatings))
            {
                XPos = X,
                YPos = Y
            };

            usersMenu.Start();
            usersMenu.Hide();
        }
        private void DelQuizRating()
        {
            int X = 5, Y = 1;
            int sel;
            string quizName = Quizzes.MixedQuizName;
            Menu quizChooseMenu = new($"   -= Оберіть назву вікторини =-", ConsoleColor.Green,
                                      ConsoleColor.DarkGray, ConsoleColor.Gray);
            foreach (var q in quizzes)
                quizChooseMenu.AddMenuItem(($"         {q.Key}", null));
            quizChooseMenu.AddMenuItem(($"         {quizName}", null));
            quizChooseMenu.XPos = X + 5;
            quizChooseMenu.YPos = Y + 1;

            do
            {
                sel = quizChooseMenu.Start();
                if (sel >=0)
                {
                    if (sel != quizChooseMenu.ItemCount - 1)
                        quizName = quizzes.QuezzesNames.ElementAt(sel);
                    if (Input.Confirm("Ви впевненні ?", "Так", "Ні", X, Console.CursorTop + 2, ConsoleColor.DarkGray, ConsoleColor.Green))
                    {
                        if (rating.DelQuizRating(quizName))
                        {
                            SLSystem.SaveRating();
                            Output.Write($"Рейтингі вікторини {quizName} видалені ...", X, Console.CursorTop, ConsoleColor.Green);
                        }
                        else Output.Write($"У вікторини  {quizName} відсутні рейтинги ...", X, Console.CursorTop, ConsoleColor.Red);
                        Console.ReadKey();
                        Output.Write(new string(' ', 60), X, Console.CursorTop);
                    }

                }

            } while (sel >= 0);
            Console.Clear();
        }
        private void DelUserRatings()
        {
            int X = 10, Y = 1;
            int sel ;
            Menu menu = new($"---- Логін --------- Ім'я ----", ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.Gray)
            {
                XPos = X,
                YPos = Y + 1
            };
            IEnumerable<User> allUsers = users.AllUsers;
            foreach (var item in allUsers)
                menu.AddMenuItem(($"     {item}", null));
            Output.Write("  -= Оберіть користувача =-", X, Y++, ConsoleColor.Red);
            do
            {
                sel = menu.Start();
                if (sel >= 0)
                {
                    if (Input.Confirm("Ви впевненні ?", "Так", "Ні", X, Console.CursorTop + 2, ConsoleColor.DarkGray, ConsoleColor.Green))
                    {
                        if (rating.DelUserRating(allUsers.ElementAt(sel).Name) != 0)
                        {
                            SLSystem.SaveRating();
                            Output.Write($"Рейтингі користувача {allUsers.ElementAt(sel).Name} видалені ...", X, Console.CursorTop, ConsoleColor.Green);
                        }
                        else Output.Write($"В користувача  {allUsers.ElementAt(sel).Name} відсутні рейтинги ...", X, Console.CursorTop, ConsoleColor.Red);
                        Console.ReadKey();
                        Output.Write(new string(' ',60), X, Console.CursorTop);
                    }

                }
            } while (sel >= 0);
            Console.Clear();
        }
        private void DelNotRegUserRatings()
        {
            int X = 10, Y = 1;
            IEnumerable<User> allUsers = users.AllUsers;
            IEnumerable<string> names = rating.GetRatingNames();
            List<string> tmp;
            if (names.Any())
            {
                if (!allUsers.Any()) tmp = names.ToList();
                else
                {
                    tmp = new();
                    foreach (var name in names)
                    {
                        bool found = false;
                        foreach (var user in allUsers)
                        {
                            if (user.Name == name)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found) tmp.Add(name);
                    }
                }
                if (tmp.Count == 0) Output.Write("Рейтинги не зареєстрованих користувачів відсутні...",X,Y, ConsoleColor.Green);
                else 
                {
                    Output.Write("Знайдено рейтинги не зареєстрованих користувачів ...", X, Y++, ConsoleColor.Green);
                    foreach (var item in tmp)
                        Output.Write(item, X + 5, Y++, ConsoleColor.Red);
                    if (Input.Confirm("Ви впевненні що хочете видалити рейтинги цих не зареєстрованих користувачів?",
                        "Так", "Ні", X, Y, ConsoleColor.DarkGray, ConsoleColor.Green))
                    {
                        foreach (var item in tmp)
                            rating.DelUserRating(item);
                        SLSystem.SaveRating();
                        Output.Write("Рейтинги не зареєстрованих користувачів видалено...", X, Y, ConsoleColor.Green);
                    }
                    else Output.Write("Натисніть будь яку клвішу для виходу...", X, Y, ConsoleColor.Green);
                }
            }
            else { Output.Write("Рейтинги відсутні...", X, Y, ConsoleColor.Red); }
            Console.ReadKey();
            Console.Clear();
        }

    }
}
