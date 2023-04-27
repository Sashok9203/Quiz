using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.IO;

namespace KnowledgeQuiz
{


    [KnownType(typeof(List<Question>))]
    internal partial class Quiz : IDisposable
    {
        private const int maxQustionCountInQuiz = 20;

        private const string mixedQuizName = "Змішана";

        private const string setingsPath = @"Settings/setings.xml";

        private const string passwordRegex = @"[^ \p{IsCyrillic}]";

        private const string loginRegex = @"[0-9a-zA-Z_]";

        private readonly Quizzes? quizzes = null;

        private readonly Users? users = null;

        private readonly Setting? setting = null;

        private bool disposedValue;

        public Quiz()
        {
            disposedValue = false;
            StringBuilder sb = new ();

            try   { if(File.Exists(setingsPath)) setting = Serializer.Deserialize<Setting>(Path.Combine(Environment.CurrentDirectory, setingsPath)); }
            catch (System.Runtime.Serialization.SerializationException)
            { sb.AppendLine($"Помилка файлу налаштувань \"{setingsPath}\""); }
            setting ??= new Setting();

            try   { if (File.Exists(setting.QuizzesPath)) quizzes = Serializer.Deserialize<Quizzes>(setting.QuizzesPath); }
            catch (System.Runtime.Serialization.SerializationException)
            { sb.AppendLine($"Помилка файлу з питаннями \"{setting.QuizzesPath}\""); }
            quizzes ??= new Quizzes();
            foreach (var item in quizzes.QuezzesPathes)
            {
                string path = Path.Combine(Environment.CurrentDirectory, item);
                if (!File.Exists(path))
                {
                    sb.AppendLine($"Файл з питаннями вікторини \"{path}\" відсутній...");
                    quizzes?.DellQuiz(item);
                }
                else
                {
                    try { Serializer.Deserialize<Question[]>(path); }
                    catch (System.Runtime.Serialization.SerializationException)
                    { sb.AppendLine($"Помилка завантаження файлу з питаннями вікторини\"{path}\" "); }
                }
            }

            try   { if (File.Exists(setting.UserPath)) users = Serializer.Deserialize<Users>(setting.UserPath); }
            catch (System.Runtime.Serialization.SerializationException)
            { sb.AppendLine($"Помилка файлу з данними про користувачів \"{setting.UserPath}\"");}
            users ??= new Users();
           
            if (sb.Length != 0)
            {
                Console.WriteLine(sb);
                Console.ReadKey();
                Console.Clear();
            }
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
                    Serializer.Serialize(setting?.QuizzesPath, quizzes);
                    Serializer.Serialize(setting?.UserPath, users);
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
