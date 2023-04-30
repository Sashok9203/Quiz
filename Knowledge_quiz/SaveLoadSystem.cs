using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    public class SaveLoadSystem
    {
        private const string setingsPath = @"setings.xml";
        private const string logPath = @"log.txt";
        private const string defQuizzesFileName = @"quizzes.xml";
        private const string defUserFileName = @"users.xml";
        private const string defRatingFileName = @"rating.xml";
        private const string defDataDir = @"ProgramData";
        private const string defQuestionDir = @"Questions";



        public string DataDir { set { setting.curentDataDir = value; } }
        public string QuestionDir { set { setting.curentQuestionDir = value; } }
        public string QuizzesFileName { set { setting.curentQuizzesFileName = value; } }
        public string UserFileName { set { setting.curentUserFileName = value; } }
        public string RatingFileName { set { setting.curentRatingFileName = value; } }



        public string QuizzesPath => Path.Combine(Environment.CurrentDirectory, DataPath, setting.curentQuizzesFileName ?? defQuizzesFileName);

        public string UsersPath => Path.Combine(Environment.CurrentDirectory, DataPath, setting.curentUserFileName ?? defUserFileName);

        public string RatingPath => Path.Combine(Environment.CurrentDirectory, DataPath, setting.curentRatingFileName ?? defRatingFileName);

        public string QuestionsPath => Path.Combine(Environment.CurrentDirectory, setting.curentQuestionDir ?? defQuestionDir);

        public string DataPath => Path.Combine(Environment.CurrentDirectory, setting.curentDataDir ?? defDataDir);

        public string SettingPath => Path.Combine(Environment.CurrentDirectory, setingsPath);

        private readonly Setting setting;

        private  Quizzes? quizzes = null;

        private  Users?   users   = null;

        private  Rating?  rating  = null;

        public  Quizzes Quizzes  => quizzes ??= LoadQuizzes();

        public  Users Users => users ??= LoadUsers();
       
        public  Rating Rating  => rating ??= LoadRating() ;


        private void saveLog(string message)
        {
            using (StreamWriter sw = new(new FileStream(Path.Combine(Environment.CurrentDirectory, logPath), FileMode.Append, FileAccess.Write)))
            {
                sw.WriteLine(message);
            }
        }

        public  SaveLoadSystem()
        {
            try
            {
                if (File.Exists(SettingPath))
                {
                    setting = Serializer.Deserialize<Setting> (SettingPath) ?? new();
                    
                }
            }
            catch (SerializationException)
            { saveLog($" {DateTime.Now} : Помилка файлу налаштувань \"{SettingPath}\""); }
            setting ??= new();
            if(!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);
            if (!Directory.Exists(QuestionsPath)) Directory.CreateDirectory(QuestionsPath);
        }

        private Quizzes LoadQuizzes()
        {
            StringBuilder sb = new();
            try { if (File.Exists(QuizzesPath)) quizzes = Serializer.Deserialize<Quizzes>(QuizzesPath) ?? new(); }
            catch (SerializationException) { sb.AppendLine($" {DateTime.Now} : Помилка файлу з питаннями \"{QuizzesPath}\"");}
            quizzes ??= new();
            if (!Directory.Exists(QuestionsPath))
            {
                Directory.CreateDirectory(QuestionsPath);
                if (quizzes.Count != 0)
                {
                    sb.Append($" {DateTime.Now} : Файли з питаннями не знайдені : ");
                    foreach (var item in quizzes)
                        sb.Append($" {item.Value} ");
                    quizzes.Clear();
                }
            }
            else
            {
                foreach (var item in quizzes)
                {
                    bool check = true;
                    string path = Path.Combine(Environment.CurrentDirectory, item.Value);
                    if (!File.Exists(path))
                    {
                        sb.AppendLine($" {DateTime.Now} : Файл з питаннями вікторини \"{path}\" відсутній...");
                        check = false;
                    }
                    else
                    {
                        try { Serializer.Deserialize<Question[]>(path); }
                        catch (SerializationException)
                        {
                            sb.AppendLine($" {DateTime.Now} : Помилка завантаження файлу з питаннями вікторини\"{path}\" ");
                            check = false;
                        }
                    }
                    if (!check) quizzes?.DellQuiz(item.Key);
                }
            }
            if(sb.Length != 0) saveLog(sb.ToString());
            return quizzes;
        }

        private Users LoadUsers()
        {
            try { if (File.Exists(UsersPath)) users = Serializer.Deserialize<Users>(UsersPath) ?? new(); }
            catch (SerializationException)
            { saveLog($" {DateTime.Now} : Помилка файлу з данними про користувачів \"{UsersPath}\""); }
            users ??= new();
            return users ;
        }

        private Rating LoadRating()
        {
            try { if (File.Exists(RatingPath)) rating = Serializer.Deserialize<Rating>(RatingPath) ?? new(); }
            catch (SerializationException)
            { saveLog($" {DateTime.Now} : Помилка файлу з рейтингами користувачів \"{RatingPath}\""); }
            rating ??= new();
            return rating ;
        }

        public IEnumerable<Question> LoadQuestions(string fileName) => Serializer.Deserialize<Question[]>(Path.Combine(QuizzesPath, fileName)) ?? Array.Empty<Question>();



        public void SaveQuizzes()
        {
            if(quizzes != null)  Serializer.Serialize(QuizzesPath, quizzes);
        }

        public void SaveUsers()
        {
            if (users != null) Serializer.Serialize(UsersPath, users);
        }

        public void SaveRating()
        {
            if (rating != null) Serializer.Serialize(RatingPath, rating);
        }

        public void SaveSettings() => Serializer.Serialize(setingsPath, setting);

        public void SaveAll()
        {
            SaveQuizzes();
            SaveUsers();
            SaveRating();
        }

    }
}
