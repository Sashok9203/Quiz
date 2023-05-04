using KnowledgeQuiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditUtility
{
    internal partial class Utility 
    {

        private const string passwordRegex = @"[^ \p{IsCyrillic}]";

        private const string loginRegex = @"[0-9a-zA-Z_]";

        private const int passwordMaxLenght = 12;

        private const int loginMaxLenght = 12;

        private Quizzes quizzes => SLSystem.Quizzes;

        private Users users => SLSystem.Users;

        private Rating rating => SLSystem.Rating;

        private readonly SaveLoadSystem SLSystem;

        public Utility()
        {
            SLSystem = new();
        }

        public void Start()
        {
            int  X = 10, Y = 1,y = Y;
            string? login, password;
            Console.Clear();
            Output.Write("-= Вхід в систему =-", X, y++, ConsoleColor.Magenta);
            login = Input.GetStringRegex("   Логін  : ", loginRegex, X, y++, loginMaxLenght, ConsoleColor.Green, ConsoleColor.Green);
            password = Input.GetStringRegex("   Пароль : ", passwordRegex, X, y++, passwordMaxLenght, ConsoleColor.Green, ConsoleColor.Green, '*');
            if (users.AdminLogPass.Login != login || (!users.AdminLogPass.ChackPassword(password)))
            {
                Output.Write("Невірний логін або пароль...", X, y++, ConsoleColor.Magenta);
                Console.ReadKey(true);
                return;
            }
            Console.Clear();
            Menu adminMenu = new($"   -= Меню адміністратора  =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
              ("        Користувачі", Users),
              ("        Рейтинги", Ratings),
              ("        Вікторини", QuizzesEdit),
              ("        Налаштування", Setting))
                {
                    XPos = X,
                    YPos = Y
                };
            adminMenu.Start();
            adminMenu.Hide();
        }
    }
}

