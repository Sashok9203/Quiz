using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
        private void printUserQuizInfo(string quizInfoName, UserQuizInfo info, int X, int Y, ConsoleColor nameColor, ConsoleColor infoColor)
        {
            Output.Write($"-= {quizInfoName} =-",X,Y++,nameColor);
            Output.Write($"Кількість питань     : ", X , Y++, infoColor);
            Output.Write($"{info.QuestionCount}", X + 23, Y - 1, ConsoleColor.Gray);
            Output.Write($"Кількість відповідей : ", X, Y++, infoColor);
            Output.Write($"{info.RightAnswerCount}", X + 23, Y - 1, info.RightAnswerCount > info.QuestionCount/3*2? ConsoleColor.Green:
                                                             info.RightAnswerCount > info.QuestionCount / 3 ? ConsoleColor.Yellow:ConsoleColor.Red);
        }

        /// <summary>
        /// Метод виводить текст в заданій координаті X
        /// </summary>
        /// <param name="question"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Color"></param>
        private void printQuestion(string question, int X, int Y, ConsoleColor Color)
        {
            int endIndex,startIndex = 0;
            do
            {
                endIndex = question.IndexOf('\n', startIndex);
                if(endIndex > 0) Output.Write(question[startIndex..endIndex], X, Y++, Color);
                else Output.Write(question[startIndex..], X, Y++, Color);
                startIndex = endIndex + 1;
            }
            while (startIndex != 0);
        }

        /// <summary>
        /// Метод надає користувачу інтерфейс для відповіді на запитання,обрати варіант відповіді і т.д.
        /// </summary>
        /// <param name="question"></param>
        /// <param name="quizName"></param>
        /// <param name="questionIndex"></param>
        /// <param name="questinCount"></param>
        /// <returns></returns>
        private bool answerQuestion(Question? question, string? quizName , int questionIndex ,int questinCount)
        {
            int X = 25, Y = 1 ,maxAnswers = 0,sel;
            string? title = null;
            var aVariants = question.AnswerVariants;
            List<string> answersVariants, answers;
            switch (question)
            {
                case MAQuestion:
                    title = "Оберіть варіанти відповіді :";
                    maxAnswers = question.AnswerVariantsCount;
                    break;
                case SAQuestion:
                    title = "Оберіть варіант відповіді :";
                    maxAnswers = 1;
                    break;
            }
            Console.Clear();
            Output.Write( $"-= {quizName} =-",X , Y++, ConsoleColor.Red);
            Output.Write($"Питання {questionIndex} / {questinCount}", X - 1, Y++, ConsoleColor.Gray);
            printQuestion(question.QuestionText, X - 15, Y, ConsoleColor.Green);
            answersVariants = new List<string>();
            for (int i = 0;i < question.AnswerVariantsCount;i++)
                 answersVariants.Add($"      {(char)(i + 97)}) {aVariants.ElementAt(i)}");
            Menu menu = new($"   {title}", X - 18, Console.CursorTop + 2, ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.Gray, answersVariants);
            answers = new List<string>();
            do
            {
               sel =  menu.Start();
                if (Input.Confirm("Ви впевненні ?", "Так", "Ні", X - 12, Console.CursorTop + 2, ConsoleColor.DarkGray, ConsoleColor.Gray))
                {
                    if (sel >= 0 && !answers.Contains(aVariants.ElementAt(sel)))
                    {
                        int y = Console.CursorTop + 2;
                        answers.Add(aVariants.ElementAt(sel));
                        maxAnswers--;
                        Output.Write($"Обрані варіанти :", X - 12, y++, ConsoleColor.Green);
                        foreach (var item in answers)
                            Output.Write($"{item}", X - 10, y++, ConsoleColor.Red);
                    }
                }
                else if (sel < 0) sel = 0;
            }
            while (maxAnswers > 0 && sel >= 0 );
            return question.AnswerQuestion(answers.ToArray());
        }

        /// <summary>
        /// Метод формує масиви запитань та викликає метод відповіді на питання answerQuestion для кожного запитання 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="quizName"></param>
        private void QuizProcess(User? user, string? quizName)
        {
            List<Question>? questions;
            int questionCount,quizPoint = 0;
            Console.Clear();
            if (quizName != mixedQuizName) questions = Utility.Shufflet(quizzes?.GetQuizeQuestions(quizName))?.ToList<Question>();
            else 
            {
                questions = new List<Question>();
                foreach (var item in quizzes?.QuezzesQuestions)
                    foreach (var i in item)
                        questions.Add(i);
                Utility.Shufflet(questions);
            }
            questionCount = (questions?.Count < maxQustionCountInQuiz) ? questions.Count : maxQustionCountInQuiz;
            for (int i = 0; i < questionCount; i++)
                if (answerQuestion(questions?[i], quizName,i + 1, questionCount)) quizPoint++;
            UserQuizInfo uqi = new(questionCount, quizPoint);
            user?.AddQuizInfo(quizName ?? "", uqi);
            Console.Clear();
            Output.Write($"-= Ваш результат =-", 10, 1);
            printUserQuizInfo(quizName, uqi,12,Console.CursorTop+1,ConsoleColor.Green,ConsoleColor.Gray);
            Console.ReadKey();
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
            login = Input.GetWord("Логін  : ", x, y++, ConsoleColor.Green);
            password = Input.GetPassword("Пароль : ", x, y++, ConsoleColor.Green, ConsoleColor.Green);
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
                login = Input.GetWord("Введіть ваш логін       : ", x, y, ConsoleColor.Green);
            } while (users?.Logins?.Contains(login) ?? false);

            password = Input.GetPassword("Введіть пароль          : ", x, ++y, ConsoleColor.Green, ConsoleColor.Green);

            DateTime date = Input.GetDateTime(null,x,y,x,++y,"Веедіть рік народження : ", 
                "Веедіть місяць народження : ","Веедіть день народження : ",ConsoleColor.Green, ConsoleColor.Green);

            users?.AddUser(new User(new LPass(login,Utility.GetHash(password)),name,date));

            Output.Write(" Ви усппішно зареєстровані в системі....", x,Console.CursorTop + 1,  ConsoleColor.Blue);

            Console.ReadKey(true);
        }

        // userMenu methods
        private void Top20(User? user)
        {
            Console.Clear();
            printQuestion("dsfgsdfgdsfgsdfgdsfgdsfgdsfgdsfgdsfgdfgdsf1\nsdfgsdfgsdfgdfgs2\nsdfgdsfgsdfgdsf3\n",10,1,ConsoleColor.Red);
            Console.ReadKey();
        }

        private void MyResults(User? user)
        { 
        }

        /// <summary>
        /// Метод формує меню вибору вікторини та запускає процес тестування
        /// </summary>
        /// <param name="user"></param>
        private void QuizStart(User? user)
        {
            int sel;
            string quizName  = mixedQuizName;
            Console.Clear();
            var qNames = new List<string>();
            for (int i = 0; i < quizzes.QuizesCount; i++)
                 qNames.Add("\t\t" + quizzes.QuezzesNames.ElementAt(i));
            qNames.Add("\t\t" + quizName);
            Menu quizChooseMenu = new($"   -= Оберіть вікторину \"{user?.LoginPass?.Login}\" =-", 10, 2, ConsoleColor.Green, 
            ConsoleColor.DarkGray, ConsoleColor.Gray, qNames);
            sel = quizChooseMenu.Start();
            if(sel >= 0) QuizProcess(user, qNames[sel].Trim());
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
                login = Input.GetWord(   "Введіть новий логін  : ", x, y, ConsoleColor.Green);
            } while (users?.Logins?.Contains(login) ?? false);
            password = Input.GetPassword("Введіть пароль       : ", x, ++y, ConsoleColor.Green, ConsoleColor.Green);
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
            oldPass = Input.GetPassword(     "Введіть пароль       : ", x, y++, ConsoleColor.Green, ConsoleColor.DarkGreen);
            if (user?.LoginPass?.ChackPassword(oldPass) ?? false)
            {
                password = Input.GetPassword("Введіть новий пароль : ", x, y++, ConsoleColor.Green, ConsoleColor.DarkGreen);
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
