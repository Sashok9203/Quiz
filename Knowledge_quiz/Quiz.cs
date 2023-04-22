using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace KnowledgeQuiz
{
    internal partial class Quiz : IDisposable
    {
        private const int maxQustionCountInQuiz = 20;

        private const string mixedQuizName = "Змішана";

        private const string setingsPath = "setings.xml";

        private Quizzes quizzes;

        private Users users;

        private Setting setting;

        private bool disposedValue;

        public Quiz()
        {
            disposedValue = false;
            try   { setting = Serializer.Deserialize<Setting>(setingsPath); }
            catch { setting = new Setting(); }
            try   { quizzes = Serializer.Deserialize<Quizzes>(setting.QuizzesPath); }
            catch { quizzes = new Quizzes(); }
            try   { users = Serializer.Deserialize<Users>(setting.UserPath); }
            catch { users = new Users(); }
        }

        public void Start()
        {
           
            Menu startMenu = new Menu("   -= Вікторина знань =-",10,1,ConsoleColor.Green,ConsoleColor.DarkGray,ConsoleColor.Gray,
                ("          Увійти", delegate () { Enter(); return false; } ),
                ("        Реєстрація", delegate () { Registration(); return false; } ),
                ("     Адмініструввання", delegate () { StartUtility("EditUtility.exe"); return false; }));
            startMenu.Start();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Serializer.Serialize(setingsPath, setting);
                    Serializer.Serialize(setting.QuizzesPath, quizzes);
                    Serializer.Serialize(setting.UserPath, users);
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
