using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KnowledgeQuiz
{
    public sealed class SaveLoadSystem
    {
        private const string LogPath        = "log.txt";
        private const string SetingsPath    = "setings.xml";
        private const string DefDataDir     = "ProgramData";
        private const string DefQuestionDir = "Questions";
        
        private readonly Setting setting;
       
        private Quizzes? quizzes;
        private Users? users;
        private Rating? rating;
                
        private string QuizzesFile  => Path.Combine(DataDir, "quizzes.xml");
        private string UsersFile    => Path.Combine(DataDir, "users.xml");
        private string RatingFile   => Path.Combine(DataDir, "rating.xml");
        
        private void saveLog(string message)
        {
            using (StreamWriter sw = new(new FileStream(LogPath, FileMode.Append, FileAccess.Write)))
            {
                sw.WriteLine($" {DateTime.Now} : {message}");
            }
        }
        private Quizzes LoadQuizzes()
        {
            quizzes = Deserialize<Quizzes>(QuizzesFile) ?? new();
            foreach (var item in quizzes)
            {
                bool check = true;
                string path = Path.Combine(QuestionDir, item.Value);
                if (!File.Exists(path))
                {
                    saveLog($"Файл з питаннями вікторини \"{path}\" відсутній...");
                    check = false;
                }
                else if (Deserialize<Question[]>(path) == null) check = false;
                if (!check) quizzes?.DellQuiz(item.Key);
            }
            return quizzes;
        }
        private Users LoadUsers() => Deserialize<Users>(UsersFile) ?? new();
        private Rating LoadRating() => rating = Deserialize<Rating>(RatingFile) ?? new();
        private bool Serialize<T>(string paths, T obj)
        {

            DataContractSerializer serializer = new(typeof(T));
            using (var fs = XmlWriter.Create(paths, new XmlWriterSettings { Encoding = Encoding.UTF8 }))
            {
                try { serializer.WriteObject(fs, obj); }
                catch (SerializationException) 
                {
                    saveLog($"Не вдалося зберегти файл {paths}");
                    return false;
                }
            }
            return true;
        }
        private T? Deserialize<T>(string paths) where T : class
        {
            if (File.Exists(paths))
            {
                DataContractSerializer serializer = new(typeof(T));
                using (XmlReader xr = XmlReader.Create(paths))
                {
                    T? tmp = null;
                    try { tmp = serializer.ReadObject(xr) as T; }
                    catch (SerializationException) { saveLog($"Помилка завантаження файлу {paths}"); }
                    return tmp;
                }
            }
            else return null;
        }

        public SaveLoadSystem()
        {
            setting = Deserialize<Setting>(SetingsPath) ?? new();
            if (!Directory.Exists(DataDir)) Directory.CreateDirectory(DataDir);
            if (!Directory.Exists(QuestionDir)) Directory.CreateDirectory(QuestionDir);
        }

        public string DataDir
        {
            get => setting.curentDataDir ?? DefDataDir;
            set
            {
                setting.curentDataDir = value;
                SaveSettings();
            }
        }
        public string QuestionDir
        {
            get => setting.curentQuestionDir ?? DefQuestionDir;
            set
            {
                setting.curentQuestionDir = value;
                SaveSettings();
            }
        }

        public Quizzes Quizzes => quizzes ??= LoadQuizzes();
        public Users   Users => users ??= LoadUsers();
        public Rating  Rating => rating ??= LoadRating();

        public void ResetReferens()
        {
            quizzes = null;
            users   = null;
            rating  = null;
        }
        public IEnumerable<Question> LoadQuestions(string fileName) => Deserialize<Question[]>(Path.Combine(QuestionDir, fileName)) ?? Array.Empty<Question>();
        public IEnumerable<Question> AllQuestions
        {
            get
            {
                List<Question> list = new();
                foreach (var item in quizzes)
                {
                    IEnumerable<Question>? des = LoadQuestions(item.Value);
                    if (des != null) list.AddRange(des);
                }
                return list;
            }
        }
        public bool SaveQuestions(Question[] questions, string fileName) => Serialize<Question[]>(Path.Combine(QuestionDir, fileName), questions);
        public void SaveQuizzes()
        {
            if (quizzes != null) Serialize(QuizzesFile, quizzes);
        }
        public void SaveUsers()
        {
            if (users != null) Serialize(UsersFile, users);
        }
        public void SaveRating()
        {
            if (rating != null) Serialize(RatingFile, rating);
        }
        public bool SaveSettings() => Serialize(SetingsPath, setting);
    }
}
