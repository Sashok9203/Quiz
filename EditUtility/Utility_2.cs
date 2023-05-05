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
            Output.Write($"  -= Користувач < {user.LoginPass.Login} > =-", X, Y++, ConsoleColor.DarkCyan);
            Output.Write($"Ім'я та прізвище  : {user.Name}", X, Y++, ConsoleColor.Green);
            Output.Write($"Дата народження   : {user.Date.ToLongDateString()}", X, Y++, ConsoleColor.Green);
            Output.Write($"Дата реєстрації   : {user.RegistrationDate.ToLongDateString()}", X, Y++, ConsoleColor.Green);
            Output.Write($"Пройдені віторини : ", X, Y, ConsoleColor.Green);
            var infos = rating.GetUserQuizInfos(user.Name);
            if (!infos.Any()) Output.Write($"відсутні...",ConsoleColor.Red);
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
        private bool setQuestionFile(string quizName)
        {
            int X = 5, Y = 1;
            int sel = -1;
            string fileName;
            bool set = false;
            var files = Directory.GetFiles(SLSystem.QuestionDir).Select(n => Path.GetFileName(n));
            if (files.Any())
            {
                Menu fileChooseMenu = new($"   -= Оберіть файл з питаннями =-", ConsoleColor.Green,
                                          ConsoleColor.DarkGray, ConsoleColor.Gray);
                fileChooseMenu.AddMenuItem(files.Select(n => ($"         {n}",(Action?)null)));
                fileChooseMenu.XPos = X + 5;
                fileChooseMenu.YPos = Y + 1;

                sel = fileChooseMenu.Start();
                if (sel >= 0)
                {
                    fileName = files.ElementAt(sel);
                    if (Input.Confirm("Ви впевненні ?", "Так", "Ні", X, Console.CursorTop + 2, ConsoleColor.DarkGray, ConsoleColor.Green))
                    {
                        quizzes.SetQuizesQuestions(quizName, fileName);
                        set = true;
                        SLSystem.SaveQuizzes();
                        Output.Write($"Вікторини {quizName} встановлені питання {fileName} ...", X, Console.CursorTop, ConsoleColor.Green);
                    }
                }
            }
            else Output.Write($"Директорія з питаннями пуста ...", X, Console.CursorTop, ConsoleColor.Red);
            if (sel >= 0) Console.ReadKey();
            Console.Clear();
            return set;
        }
        private Question createQuestion(int index)
        {
            int X = 10, Y = 2, sel, maxAnswers;
            List<string> aVariants = new(),answers = new();
            Question question;
            Console.Clear();
            Output.Write($"Питання {index}", X + 19, Y++, ConsoleColor.Blue);
            string questionText = Input.GetText("-= Введіть текст питання =-", X, Y, ConsoleColor.Green, ConsoleColor.Blue);
            Y = Console.CursorTop - 1;
            Output.Write("-= Введіть віріант(и) відповіді =-", X, Y++, ConsoleColor.Green);
            do { aVariants.Add(Input.GetString(null,X + 4, Y++ ,default));}
            while (Input.Confirm("Бажаєте дотати ще один варіант відповіді?","Так", "Ні", X, Console.CursorTop + 1, ConsoleColor.DarkGray, ConsoleColor.Green));
            Y = Console.CursorTop + 1;
            Menu menu = new("-= Оберіть відповідь(і) =-", ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.Gray,
                aVariants.Select(n => $"   {n}"))
            {
                XPos = X,
                YPos = Y
            };
            maxAnswers = aVariants.Count;
            do
            {
                sel = menu.Start();
                if (sel >= 0)
                {
                    if (!answers.Contains(aVariants.ElementAt(sel)))
                    {
                        maxAnswers--;
                        if (maxAnswers == 0)
                        {
                            if (Input.Confirm("Ви впевненні ?", "Так", "Ні", X, Console.CursorTop + 2, ConsoleColor.DarkGray, ConsoleColor.Gray))
                            {
                                answers.Add(aVariants.ElementAt(sel));
                                sel = -1;
                            }
                            else maxAnswers++;
                        }
                        else
                        {
                            answers.Add(aVariants.ElementAt(sel));
                            menu.SetItemString(sel, "*" + menu.GetItemString(sel)[1..]);
                        }
                    }
                    else
                    {
                        answers.Remove(aVariants.ElementAt(sel));
                        maxAnswers++;
                        menu.SetItemString(sel, " " + menu.GetItemString(sel)[1..]);
                    }
                }
                else if (!Input.Confirm("Ви впевненні ?", "Так", "Ні", X , Console.CursorTop + 2, ConsoleColor.DarkGray, ConsoleColor.Gray)) sel = 0;
            }
            while (sel >= 0);
            if (answers.Count == 1) question = new SAQuestion(questionText, aVariants.ToArray());
            else question = new MAQuestion(questionText, aVariants.ToArray());
            foreach (var item in answers)
                  question.AddAnswer(item);
            return question;
        }
        private Question[] createQuestions()
        {
            int index = 0;
            List<Question> questions = new();
            do { questions.Add(createQuestion(index++)); }
            while (Input.Confirm("Бажаєте додати ще одне питання?", "Так", "Ні", Console.CursorLeft, Console.CursorTop, ConsoleColor.DarkGray, ConsoleColor.Green));
            Console.Clear();
            return questions.ToArray();
        }
        private string? ChooseQuizName()
        {
            int X = 5, Y = 1;
            int sel;
            Menu quizChooseMenu = new($"   -= Оберіть  вікторину =-", ConsoleColor.Green,
                                      ConsoleColor.DarkGray, ConsoleColor.Gray);
            quizChooseMenu.AddMenuItem(quizzes.Select(n=>($"         {n.Key}", (Action?)null)));
            quizChooseMenu.XPos = X + 5;
            quizChooseMenu.YPos = Y + 1;
            
            sel = quizChooseMenu.Start();
            if (sel >= 0)
            {
                quizChooseMenu.Hide();
                return quizzes.QuezzesNames.ElementAt(sel);
            }

            return null;
        }


        private void QuizzesEdit()
        {
            int X = 10, Y = 1;
            Menu settingMenu = new($"   -= Вікторини =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
              ("  Видалити вікторину", DelQuiz),
              ("  Додати вікторину", AddQuiz),
              ("  Завантажити питання",  LoadQuestionsFile),
              ("  Створити файл з питаннями", () => { CreateQuestionFile(); }),
              ("  Додати питання", AddQuestion),
              ("  Видалити питання", DelQuestion))
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
                Menu quizChooseMenu = new("   -= Оберіть  вікторину =-", ConsoleColor.Green,
                                      ConsoleColor.DarkGray, ConsoleColor.Gray)
                {
                    XPos = X + 5,
                    YPos = Y + 1
                };

                do
                {
                    quizChooseMenu.AddMenuItem(quizzes.Select(n=>($"         {n.Key}", (Action?)null)));
                    sel = quizChooseMenu.Start();
                    if (sel >= 0)
                    {
                        quizName = quizzes.QuezzesNames.ElementAt(sel);
                        if (Input.Confirm("Ви впевненні ?", "Так", "Ні", X, Console.CursorTop + 2, ConsoleColor.DarkGray, ConsoleColor.Green))
                        {
                            if (quizzes.DellQuiz(quizName))
                            {
                                rating.DelQuizRating(quizName);
                                if (quizzes.Count == 0) sel = -1;
                                SLSystem.SaveQuizzes();
                                SLSystem.SaveRating();
                                Output.Write($"Вікторина {quizName} та її рейтинги видалені ...", X, Console.CursorTop, ConsoleColor.Green);
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
            Menu quizChooseMenu = new("   -= Оберіть  вікторину =-", ConsoleColor.Green,
                                      ConsoleColor.DarkGray, ConsoleColor.Gray);
            quizChooseMenu.AddMenuItem(quizzes.Select(n => ($"         {n.Key}", (Action?)null)));
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
        private string CreateQuestionFile()
        {
            Question[] questions = createQuestions();
            string fileName = Input.GetWord("Введіть назву файлу під якою буде збережено файл : ", Console.CursorLeft, Console.CursorTop + 1, ConsoleColor.Green);
            if (SLSystem.SaveQuestions(questions.ToArray(), fileName))
                Output.Write($" {questions.Length} питань збережено у файл {fileName}.", Console.CursorLeft, Console.CursorTop + 1, ConsoleColor.Green);
            else Output.Write($"Сталася помилка при збережені питань у файл {fileName}.", Console.CursorLeft, Console.CursorTop + 1, ConsoleColor.Red);
            Console.ReadKey();
            Console.Clear();
            return fileName;
        }
        private void AddQuiz()
        {
            int X = 10, Y = 1,sel;
            string fileName;
            bool set = false;
            string quizName = Input.GetString("Введіть назву вікторини : ", X, Y, ConsoleColor.Green);
            Output.Write(new string(' ', 60), X, Y);
            Menu menu = new($"   -= Оберіть варіант =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
              (" Завантажити файл з питаннями",null ),
              (" Створити файл з питаннями",null))
            {  
                XPos = X,
                YPos = Y
            };
            do
            {
                sel = menu.Start();
                if (sel >= 0)
                {
                    switch (sel)
                    {
                        case 0:
                            menu.Hide();
                            set = setQuestionFile(quizName);
                            break;
                        case 1:
                            menu.Hide();
                            fileName = CreateQuestionFile();
                            set = quizzes.AddQuiz(quizName, fileName);
                            if (set)
                            {
                                Output.Write($"Вікторини  {quizName} додана до списку вікторин", Console.CursorLeft, Console.CursorTop, ConsoleColor.Green);
                                SLSystem.SaveQuizzes();
                            }
                            else Output.Write($"Сталася помилка при додаванні вікторини  {quizName} додана до списку вікторин", Console.CursorLeft, Console.CursorTop, ConsoleColor.Green);
                            break;
                    }

                }
            } while (sel >= 0 && !set);
            Console.Clear();
        }
        private void AddQuestion()
        {
            string? quizName = ChooseQuizName();
            if (quizName == null) return;
            string? path = quizzes.GetQuizeQuestionsPath(quizName);
            List<Question> questions = SLSystem.LoadQuestions(path).ToList();
            questions.AddRange(createQuestions());
            SLSystem.SaveQuestions(questions.ToArray(), path ?? "");
            Output.Write($"Питання додані до списку питань вікторини \"{quizName}\"", Console.CursorLeft, Console.CursorTop, ConsoleColor.Green);
            Console.ReadKey();
            Console.Clear();
        }
        private void DelQuestion()
        {
            int X = 5, Y = 1, sel;
            string? quizName = ChooseQuizName();
            if (quizName == null) return;
            string? path = quizzes.GetQuizeQuestionsPath(quizName);
            List<Question> questions = SLSystem.LoadQuestions(path).ToList();
                 
            if (questions.Count != 0)
            {
                bool removed = true;
                do
                {
                    if (removed)
                    {
                        int y = Y;
                        sel = 0;
                        Console.SetCursorPosition(X, y);
                        foreach (var item in questions)
                        {
                            Output.Write($"Питання {++sel}", X, y++, ConsoleColor.Red);
                            Output.WriteText(item.QuestionText, X + 4, y++, ConsoleColor.Green);
                            y = Console.CursorTop + 2;
                        }
                    }
                    Output.Write($"Введіть номер питання для виделення 1 - {questions.Count} (0 - вихід) : ", X, Console.CursorTop + 2, ConsoleColor.DarkCyan);
                    if (!removed)
                    {
                        Console.Write("   ");
                        Console.SetCursorPosition(Console.CursorLeft - 3 , Console.CursorTop);
                    }
                    sel = Input.GetInt(0, questions.Count);
                    if (sel > 0)
                    {
                        if (Input.Confirm("Ви впевненні ?", "Так", "Ні", X, Console.CursorTop, ConsoleColor.DarkGray, ConsoleColor.DarkCyan))
                        {
                            questions.RemoveAt(sel - 1);
                            Console.Clear();
                            removed = true;
                        }
                        else
                        {
                            Console.SetCursorPosition(X, Console.CursorTop - 3);
                            removed = false;
                        }
                    }

                } while (sel > 0 && questions.Count != 0);
            }
            if (questions.Count == 0)
            {
                Output.Write($"Питання  відсутні ...", X, Y, ConsoleColor.Green);
                Console.ReadKey();
            }
            SLSystem.SaveQuestions(questions.ToArray(), path ?? "");
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
            }
            catch { Output.Write($"Не вірний шлях  : {newDir}", x, y++, ConsoleColor.Red); }
            Console.ReadKey();
            Console.Clear();
        }

        private void Users()
        {
            int X = 10, Y = 1;
            Menu usersMenu = new("    -= Користувачі =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
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
            int sel;
            IEnumerable<User> allUsers = users.AllUsers;
            if (allUsers.Any())
            {
                Menu menu = new("---- Логін ---------- Ім'я ----", ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.Gray)
                {
                    XPos = X,
                    YPos = Y + 1
                };

                menu.AddMenuItem(allUsers.Select(n => ($"    {n}", (Action?)null)));

                do
                {
                    Output.Write("    -= Оберіть користувача =-", X, Y, ConsoleColor.Red);
                    sel = menu.Start();
                    if (sel >= 0)
                    {
                        menu.Hide();
                        Output.Write(new string(' ', 29), X, Y);
                        showUser(allUsers.ElementAt(sel));
                    }
                } while (sel >= 0);
            }
            else 
            {
                Output.Write($"Зареєстровані користувачі відсутні ...", X, Y, ConsoleColor.Red);
                Console.ReadKey();
            }
            Console.Clear();
        }
        private void DelUser()
        {
            int X = 10, Y = 1;
            int sel = 0;
            var allUsers = users.AllUsers;
            if (allUsers.Any())
            {

                Output.Write("    -= Оберіть користувача =-", X, Y, ConsoleColor.Red);
                Menu menu = new("---- Логін ---------- Ім'я ----", ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.Gray)
                {
                    XPos = X,
                    YPos = Y + 1
                };

                do
                {
                    menu.Clear();
                    int x = X, y = Y;
                    if (users.Count != 0)
                    {
                        menu.AddMenuItem(allUsers.Select(n => ($"    {n}", (Action?)null)));
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
            }
            else
            {
                Output.Write($"Зареєстровані користувачі відсутні ...", X, Y, ConsoleColor.Red);
                Console.ReadKey();
            }
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
            quizChooseMenu.AddMenuItem(quizzes.Select(n=>($"         {n.Key}",(Action?)null)));
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
            var allUsers = users.AllUsers;
            if (allUsers.Any())
            {
                Menu menu = new("---- Логін ---------- Ім'я ----", ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.Gray)
                {
                    XPos = X,
                    YPos = Y + 1
                };
                menu.AddMenuItem(allUsers.Select(n => ($"    {n}", (Action?)null)));
                Output.Write("    -= Оберіть користувача =-", X, Y++, ConsoleColor.Red);
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
                            else Output.Write($"В користувача {allUsers.ElementAt(sel).Name} відсутні рейтинги ...", X, Console.CursorTop, ConsoleColor.Red);
                            Console.ReadKey();
                            Output.Write(new string(' ', 60), X, Console.CursorTop);
                        }

                    }
                } while (sel >= 0);
            }
            else
            {
                Output.Write($"Зареєстровані користувачі відсутні ...", X, Y, ConsoleColor.Red);
                Console.ReadKey();
            }
            Console.Clear();
        }
        private void DelNotRegUserRatings()
        {
            int X = 10, Y = 1;
            if (!rating.IsEmpty)
            {
                string[] UnNames = rating.GetRatingUserNames().Except(users.AllUsers.Select(n => n.Name)).ToArray();
                if (!UnNames.Any()) Output.Write("Рейтинги не зареєстрованих користувачів відсутні...",X,Y, ConsoleColor.Green);
                else 
                {
                    Output.Write("Знайдено рейтинги не зареєстрованих користувачів ...", X, Y++, ConsoleColor.Green);
                    foreach (var item in UnNames)
                        Output.Write(item, X + 5, Y++, ConsoleColor.Red);
                    if (Input.Confirm("Ви впевненні що хочете видалити рейтинги цих не зареєстрованих користувачів?",
                        "Так", "Ні", X, Y, ConsoleColor.DarkGray, ConsoleColor.Green))
                    {
                        foreach (var item in UnNames)
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
