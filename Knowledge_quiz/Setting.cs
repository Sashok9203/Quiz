
using System.Runtime.Serialization;


namespace KnowledgeQuiz
{
    
    [Serializable]
    public class Setting : ISerializable
    {
        public  string? curentQuizzesFileName = null;
        public  string? curentUserFileName = null;
        public  string? curentRatingFileName = null;
        public  string? curentDataDir = null;
        public  string? curentQuestionDir = null;

        public Setting() { }

        public Setting(SerializationInfo info, StreamingContext context)
        {
            curentQuizzesFileName = info.GetString("CurentQuizzesFileName");
            curentUserFileName = info.GetString("CurentUserFileName");
            curentRatingFileName = info.GetString("CurentRatingFileName");
            curentDataDir = info.GetString("CurentDataDir"); ;
            curentQuestionDir = info.GetString("CurentQuestionDir"); ;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CurentQuizzesFileName", curentQuizzesFileName);
            info.AddValue("CurentUserFileName", curentUserFileName);
            info.AddValue("CurentRatingFileName", curentRatingFileName);
            info.AddValue("CurentDataDir", curentDataDir);
            info.AddValue("CurentQuestionDir", curentQuestionDir);
        }
    }
}
