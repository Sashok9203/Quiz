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
       
        private readonly string logPath ;
        private readonly string defDataDir ;
        private readonly string defQuestionDir ;

        public string DataDir
        {
            get => setting.curentDataDir ?? defDataDir;
            set => setting.curentDataDir = value;  
        }

        public string QuestionDir
        { 
            get => setting.curentQuestionDir ?? defQuestionDir;
            set => setting.curentQuestionDir = value;
        }

        public string SetingsPath { get; }

        public string QuizzesFile => Path.Combine(DataDir, "quizzes.xml");

        public string UsersFile   => Path.Combine(DataDir, "users.xml");

        public string RatingFile  => Path.Combine(DataDir, "rating.xml");
      

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
            logPath = Path.Combine(Environment.CurrentDirectory, "log.txt");
            SetingsPath = Path.Combine(Environment.CurrentDirectory, "setings.xml");
            defDataDir = Path.Combine(Environment.CurrentDirectory, @"ProgramData");
            defQuestionDir = Path.Combine(Environment.CurrentDirectory, @"Questions");
            try
            {
                if (File.Exists(SetingsPath))
                {
                    setting = Serializer.Deserialize<Setting> (SetingsPath) ?? new();
                    
                }
            }
            catch (SerializationException)
            { saveLog($" {DateTime.Now} : Помилка файлу налаштувань \"{SetingsPath}\""); }
            setting ??= new();
            if(!Directory.Exists(DataDir)) Directory.CreateDirectory(DataDir);
            if (!Directory.Exists(QuestionDir)) Directory.CreateDirectory(QuestionDir);
        }

        private Quizzes LoadQuizzes()
        {
            StringBuilder sb = new();
            try { if (File.Exists(QuizzesFile)) quizzes = Serializer.Deserialize<Quizzes>(QuizzesFile) ?? new(); }
            catch (SerializationException) { sb.AppendLine($" {DateTime.Now} : Помилка файлу з питаннями \"{QuizzesFile}\"");}
            quizzes ??= new();
            if (!Directory.Exists(QuestionDir))
            {
                Directory.CreateDirectory(QuestionDir);
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
            try { if (File.Exists(UsersFile)) users = Serializer.Deserialize<Users>(UsersFile) ?? new(); }
            catch (SerializationException)
            { saveLog($" {DateTime.Now} : Помилка файлу з данними про користувачів \"{UsersFile}\""); }
            users ??= new();
            return users ;
        }

        private Rating LoadRating()
        {
            try { if (File.Exists(RatingFile)) rating = Serializer.Deserialize<Rating>(RatingFile) ?? new(); }
            catch (SerializationException)
            { saveLog($" {DateTime.Now} : Помилка файлу з рейтингами користувачів \"{RatingFile}\""); }
            rating ??= new();
            return rating ;
        }

        public IEnumerable<Question> LoadQuestions(string fileName) => Serializer.Deserialize<Question[]>(Path.Combine(QuizzesFile, fileName)) ?? Array.Empty<Question>();



        public void SaveQuizzes()
        {
            if(quizzes != null)  Serializer.Serialize(QuizzesFile, quizzes);
        }

        public void SaveUsers()
        {
            if (users != null) Serializer.Serialize(UsersFile, users);
        }

        public void SaveRating()
        {
            if (rating != null) Serializer.Serialize(RatingFile, rating);
        }

        public void SaveSettings() => Serializer.Serialize(SetingsPath, setting);

        public void SaveAll()
        {
            SaveQuizzes();
            SaveUsers();
            SaveRating();
        }

    }
}
