
using System.Runtime.Serialization;


namespace KnowledgeQuiz
{
    [KnownType(typeof(LPass))]
    [Serializable]
    public class Setting : ISerializable
    {
        
        private const string defAdminLogin  = "admin";
        private const string defAdminPass   = "admin";



        public  string? curentDataDir;
        public  string? curentQuestionDir;
        public  string? curentQuizzesFileName;
        public  string? curentUserFileName;
        public  string? curentRatingFileName;


        
        


        public LPass AdminLogPass { get; private set; }

        public Setting()
        {
            AdminLogPass = new(defAdminLogin, defAdminPass);
            curentQuizzesFileName = null;
            curentUserFileName = null;
            curentRatingFileName = null;
            curentDataDir = null;
            curentQuestionDir = null;
    }

        public Setting(SerializationInfo info, StreamingContext context)
        {
            AdminLogPass = info.GetValue("AdminPassLog", typeof(LPass)) as LPass ?? new(defAdminLogin, defAdminPass);
            curentQuizzesFileName = info.GetString("CurentQuizzesFileName");
            curentUserFileName = info.GetString("CurentUserFileName");
            curentRatingFileName = info.GetString("CurentRatingFileName");
            curentDataDir = info.GetString("CurentDataDir"); ;
            curentQuestionDir = info.GetString("CurentQuestionDir"); ;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AdminPassLog", AdminLogPass);
            info.AddValue("CurentQuizzesFileName", curentQuizzesFileName);
            info.AddValue("CurentUserFileName", curentUserFileName);
            info.AddValue("CurentRatingFileName", curentRatingFileName);
            info.AddValue("CurentDataDir", curentDataDir);
            info.AddValue("CurentQuestionDir", curentQuestionDir);
        }
    }
}
