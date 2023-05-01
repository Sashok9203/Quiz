using KnowledgeQuiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditUtility
{
    internal partial class Utility : IDisposable
    {

        private const string passwordRegex = @"[^ \p{IsCyrillic}]";

        private const string loginRegex = @"[0-9a-zA-Z_]";

        private Quizzes quizzes => SLSystem.Quizzes;

        private Users users => SLSystem.Users;

        private Rating rating => SLSystem.Rating;

        private readonly SaveLoadSystem SLSystem;

        private bool disposedValue;

        public Utility()
        {
            SLSystem = new();
            disposedValue = false;
        }
        public void Start()
        {
            int  X = 10, Y = 1,y = Y;
            string? login, password;
            Console.Clear();
            Output.Write("-= Вхід в систему =-", X, y++, ConsoleColor.Magenta);
            login = Input.GetStringRegex("   Логін  : ", loginRegex, X, y++, ConsoleColor.Green, ConsoleColor.Green);
            password = Input.GetStringRegex("   Пароль : ", passwordRegex, X, y++, ConsoleColor.Green, ConsoleColor.Green, '*');
            if (users.AdminLogPass.Login != login || (!users.AdminLogPass.ChackPassword(password)))
            {
                Output.Write("Невірний логін або пароль...", X, y++, ConsoleColor.Magenta);
                Console.ReadKey(true);
                return;
            }
            Output.Write($"Вітаємо в системі ...", X, ++y, ConsoleColor.Green);
            Console.ReadKey(true);
            Console.Clear();
            Menu adminMenu = new($"   -= Меню адміністратора  =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
              ("    Користувачі", Users),
              ("    Рейтинги", () => { return false; } ),
              ("    Вікторини", () => { return false; } ),
              ("    Налаштування", Setting))
                {
                    XPos = X,
                    YPos = Y
                };
            adminMenu.Start();
            adminMenu.Hide();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SLSystem.SaveAll();
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения
                // TODO: установить значение NULL для больших полей
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

