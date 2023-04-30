
using System.Diagnostics;


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
            Output.Write($"Вікторина \"{info.QuizName}\"",X + 3,Y++,nameColor);
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
            Output.Write($"{rating?.GetUserPlace(info.QuizName,info.UserName)}", X + 23, Y - 1, ConsoleColor.Gray);
        }

        /// startMenu methods
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
            Output.Write("-= Вхід в систему =-", x, y++ , ConsoleColor.Magenta);
            login = Input.GetStringRegex("Логін  : ", loginRegex, x, y++, ConsoleColor.Green, ConsoleColor.Green);
            password = Input.GetStringRegex("Пароль : ", passwordRegex, x, y++, ConsoleColor.Green, ConsoleColor.Green,'*');
            curentUser = users.GetUser(login);
            if (curentUser == null || (!curentUser?.LoginPass?.ChackPassword(password) ?? false))
            {
                Output.Write("Невірний логін або пароль...", x, y++, ConsoleColor.Magenta);
                Console.ReadKey(true);
                return;
            }
            Output.Write($"Вітаємо в системі {curentUser?.Name} ...", x, ++y, ConsoleColor.Green);
            Console.ReadKey(true);
            Console.Clear();
            Menu userMenu = new ($"   -= Меню користувача \"{curentUser?.LoginPass?.Login}\" =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
                ("    Топ 20", delegate () { Top20(curentUser); return false; } ),
                ("    Мої результати ", delegate () { MyResults(curentUser); return false; } ),
                ("    Стартувати вікторину", delegate () { QuizStart(curentUser); return false; } ),
                ("    Налаштування", delegate () { Setting(curentUser); return false; } ));
            userMenu.XPos = 10;
            userMenu.YPos = 1;
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

            Output.Write("-= Реєстрація нового користувача =-", x - 4, y - 2,ConsoleColor.Magenta);

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
            } while (users.Logins?.Contains(login) ?? false);

            password = Input.GetStringRegex("Введіть пароль          : ",passwordRegex, x, ++y, ConsoleColor.Green, ConsoleColor.Green,'*');

            DateTime date = Input.GetDateTime(null,x,y,x,++y,"Веедіть рік народження : ", 
                "Веедіть місяць народження : ","Веедіть день народження : ",ConsoleColor.Green, ConsoleColor.Green);

            users.AddUser(new User(new LPass(login,password),name,date));

            SLSystem.SaveUsers();

            Output.Write(" Ви усппішно зареєстровані в системі....", x,Console.CursorTop + 1,  ConsoleColor.Blue);

            Console.ReadKey(true);
        }


        // userMenu methods
        /// <summary>
        /// Функія виводить рейтинг топ 20
        /// </summary>
        /// <param name="user"></param>
        private void Top20(User? user)
        {
            int X = 5, Y = 1;
            string quizName = Quizzes.MixedQuizName;
            Menu quizChooseMenu = new($"   -= Оберіть вікторину \"{user?.LoginPass?.Login}\" =-", ConsoleColor.Green,
                                      ConsoleColor.DarkGray, ConsoleColor.Gray);
            foreach (var q in quizzes)
                quizChooseMenu.AddMenuItem(($"\t\t {q.Key}", null));
            quizChooseMenu.AddMenuItem(($"\t\t {quizName}", null));
            Console.Clear();
            quizChooseMenu.XPos = X + 5;
            quizChooseMenu.YPos = Y + 1;
            int sel = quizChooseMenu.Start();
            Console.Clear();
            if (sel < 0) return;
            if(sel!= quizChooseMenu.ItemCount-1)
            quizName = quizzes.QuezzesNames.ElementAt(sel);
            var infos = rating?.GetQuizInfos(quizName);
            if (infos != null)
            {
                Output.Write("-= TOP 20 =-", X + 10, Y++, ConsoleColor.Green);
                Output.Write($"-= \"{quizName}\" =-", X + 7, Y++, ConsoleColor.Green);
                Y++;
                Output.Write("Місце     Імя      Кв.\\Пв.     Час", X, Y++, ConsoleColor.Green);
                Output.Write("-----------------------------------", X, Y++, ConsoleColor.Red);
                int index = 1;

                foreach (var item in infos)
                {
                    Output.Write($"{index++}     {item}", X + 2, Y++, ConsoleColor.Blue);
                }
            }
            else Output.Write("Інформація відсутня", X, Y++, ConsoleColor.Green);
            Console.ReadKey();
        }

        private void MyResults(User? user)
        {
            int X = 3, Y = 1;
            Console.Clear();
            IEnumerable<UserQuizInfo>? infos = rating.GetUserQuizInfos(user?.Name ?? "");
            if (infos != null)
            {
                foreach (var item in infos)
                {
                    printUserQuizInfo(item, X, Y + Console.CursorTop + 1 , ConsoleColor.Red, ConsoleColor.Green);
                }
            }
            else Output.Write("Результати відсутні", X, Y, ConsoleColor.Red);
            Console.ReadKey();
        }

        /// <summary>
        /// Метод формує меню вибору вікторини та запускає процес тестування
        /// </summary>
        /// <param name="user"></param>
        private void QuizStart(User? user)
        {
            int X = 5, Y = 1;
            string quizName = Quizzes.MixedQuizName;
            Test test;
            Console.Clear();

            Menu quizChooseMenu = new($"   -= Оберіть вікторину \"{user?.LoginPass?.Login}\" =-", ConsoleColor.Green,
                                      ConsoleColor.DarkGray, ConsoleColor.Gray);
            foreach (var q in quizzes)
                quizChooseMenu.AddMenuItem(($"\t\t {q.Key}", null));
            quizChooseMenu.AddMenuItem(($"\t\t {quizName}", null));
            quizChooseMenu.XPos = X + 5;
            quizChooseMenu.YPos = Y + 1;
            int sel = quizChooseMenu.Start();
            if (sel < 0) return;
            if (sel != quizChooseMenu.ItemCount - 1)
                quizName = quizzes.QuezzesNames.ElementAt(sel);

            try { test = quizzes?.GetTest(user?.Name,quizName); }
            catch (Exception ex)
            {
                Output.Write(ex.Message, 10, 1);
                Console.ReadKey();
                return;
            }
            
           

            UserQuizInfo? qi = test?.Start();

            rating?.AddQuizInfo(qi);

            SLSystem.SaveRating();


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
             userSettingMenu = new($"   -= Налаштування \"{user?.LoginPass?.Login}\" =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
                ("       Змінити пароль", delegate () { ChangePassword(user); return true; } ),
                ("       Змінити дату",   delegate () { ChangeDate(user); return true; } ));
            userSettingMenu.XPos = 10;
            userSettingMenu.YPos = 1;
            userSettingMenu.Start();
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
                SLSystem.SaveUsers();
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
            SLSystem.SaveUsers();
            Console.ReadKey();
        }


    }
}
