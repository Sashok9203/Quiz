﻿
using System.Text;
using System.Runtime.Serialization;
using System.Reflection.PortableExecutable;


namespace KnowledgeQuiz
{


    [KnownType(typeof(List<Question>))]
    internal partial class Quiz : IDisposable
    {
       
        private const string passwordRegex = @"[^ \p{IsCyrillic}]";

        private const string loginRegex = @"[0-9a-zA-Z_]";

        private const int passwordMaxLenght = 12;

        public const int loginMaxLenght = 12;

        private  Quizzes quizzes => SLSystem.Quizzes;

        private  Users users  => SLSystem.Users;

        private  Rating rating => SLSystem.Rating;

        private readonly SaveLoadSystem SLSystem;

        private bool disposedValue;

        public Quiz()
        {
            SLSystem = new();

            disposedValue = false;
        }

        public void Start()
        {

            Menu startMenu = new Menu("   -= Вікторина знань =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
                ("       Увійти",  () =>  Enter()),
                ("     Реєстрація",  () => Registration()),
                ("  Адмініструввання",  () => { StartUtility("EditUtility.exe") ; }));
            startMenu.XPos = 10;
            startMenu.YPos = 1;
            startMenu.Start();
            startMenu.Hide();
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
