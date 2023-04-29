﻿
using System.Runtime.Serialization;


namespace KnowledgeQuiz
{
    [KnownType(typeof(LPass))]
    [Serializable]
    public class Setting : ISerializable
    {
        
        private const string defAdminLogin  = "admin";
        private const string defAdminPass   = "admin";

        private const string defQuizzesPath = @"Settings/data.xml";
        private const string defUserPath    = @"Settings/users.xml";
        private const string defRatingPath  = @"Settings/rating.xml";


        private  string? curentQuizzesPath;
        private  string? curentUserPath;
        private  string? curentRatingPath;

        public string QuizzesPath
        {
            get => Path.Combine(Environment.CurrentDirectory, curentQuizzesPath ?? defQuizzesPath);
            set => curentQuizzesPath = value;
        }

        public string UserPath
        {
            get => Path.Combine(Environment.CurrentDirectory, curentUserPath ?? defUserPath);
            set => curentUserPath = value;
        }

        public string RatingPath
        {
            get => Path.Combine(Environment.CurrentDirectory, curentRatingPath ?? defRatingPath);
            set => curentRatingPath = value;
        }



        public LPass AdminLogPass { get; private set; }

        public Setting()
        {
            AdminLogPass = new(defAdminLogin, defAdminPass);
            curentQuizzesPath = null;
            curentUserPath = null;
            curentRatingPath = null;
        }

        public Setting(SerializationInfo info, StreamingContext context)
        {
            AdminLogPass = info.GetValue("AdminPassLog", typeof(LPass)) as LPass ?? new(defAdminLogin, defAdminPass); 
            curentQuizzesPath = info.GetString("CurentQuizzesPath");
            curentUserPath = info.GetString("CurentUserPath");
            curentRatingPath = info.GetString("CurentRatingPath");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AdminPassLog", AdminLogPass);
            info.AddValue("CurentQuizzesPath", curentQuizzesPath);
            info.AddValue("CurentUserPath", curentUserPath);
            info.AddValue("CurentRatingPath", curentRatingPath);
        }
    }
}
