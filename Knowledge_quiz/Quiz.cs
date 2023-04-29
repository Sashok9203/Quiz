
using System.Text;
using System.Runtime.Serialization;


namespace KnowledgeQuiz
{


    [KnownType(typeof(List<Question>))]
    internal partial class Quiz : IDisposable
    {
        private const string setingsPath = @"Settings/setings.xml";

        private const string logPath = @"Settings/log.txt";

        private const string passwordRegex = @"[^ \p{IsCyrillic}]";

        private const string loginRegex = @"[0-9a-zA-Z_]";

        private readonly Quizzes? quizzes = null;

        private readonly Users? users = null;

        private readonly Setting? setting = null;

        private readonly Rating? rating = null;

        private bool disposedValue;

        public Quiz()
        {
            disposedValue = false;
            StringBuilder sb = new ();
            sb.AppendLine("Start");
            sb.AppendLine(DateTime.Now.ToLongTimeString());
            sb.AppendLine(DateTime.Now.ToLongDateString());

            try   { if(File.Exists(setingsPath)) setting = Serializer.Deserialize<Setting>(Path.Combine(Environment.CurrentDirectory, setingsPath)); }
            catch (SerializationException)
            { sb.AppendLine($"Помилка файлу налаштувань \"{setingsPath}\""); }
            setting ??= new ();

            try   { if (File.Exists(setting.QuizzesPath)) quizzes = Serializer.Deserialize<Quizzes>(setting.QuizzesPath); }
            catch (SerializationException)
            { sb.AppendLine($"Помилка файлу з питаннями \"{setting.QuizzesPath}\""); }
            quizzes ??= new ();

            foreach (var item in quizzes.QuezzesPathes)
            {
                bool check = true;
                string path = Path.Combine(Environment.CurrentDirectory, item);
                if (!File.Exists(path))
                {
                    sb.AppendLine($"Файл з питаннями вікторини \"{path}\" відсутній...");
                    check = false;
                }
                else
                {
                    try { Serializer.Deserialize<Question[]>(path); }
                    catch (SerializationException)
                    { 
                        sb.AppendLine($"Помилка завантаження файлу з питаннями вікторини\"{path}\" ");
                        check = false;
                    }
                }
                if(!check) quizzes?.DellQuiz(item);
            }

            try   { if (File.Exists(setting.UserPath)) users = Serializer.Deserialize<Users>(setting.UserPath); }
            catch (SerializationException)
            { sb.AppendLine($"Помилка файлу з данними про користувачів \"{setting.UserPath}\"");}
            users ??= new ();

            try { if (File.Exists(setting.RatingPath)) rating = Serializer.Deserialize<Rating>(setting.RatingPath); }
            catch (SerializationException)
            { sb.AppendLine($"Помилка файлу з рейтингами користувачів \"{setting.RatingPath}\""); }
            rating ??= new ();


            using (StreamWriter sw = new(new FileStream(Path.Combine(Environment.CurrentDirectory, logPath), FileMode.Append, FileAccess.Write)))
            {
                sw.WriteLine(sb);
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
                    Serializer.Serialize(setting?.RatingPath, rating);

                    StringBuilder sb = new();
                    sb.AppendLine("Stop");
                    sb.AppendLine(DateTime.Now.ToLongTimeString());
                    sb.AppendLine(DateTime.Now.ToLongDateString());
                    using (StreamWriter sw = new(new FileStream(Path.Combine(Environment.CurrentDirectory, logPath), FileMode.Append, FileAccess.Write)))
                    {
                        sw.WriteLine(sb);
                    }
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
