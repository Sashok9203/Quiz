
using System.Runtime.Serialization;


namespace KnowledgeQuiz
{
    
    [Serializable]
    public class Setting : ISerializable
    {
        public  string? curentDataDir = null;
        public  string? curentQuestionDir = null;

        public Setting() { }

        public Setting(SerializationInfo info, StreamingContext context)
        {
            curentDataDir = info.GetString("CurentDataDir"); ;
            curentQuestionDir = info.GetString("CurentQuestionDir"); ;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CurentDataDir", curentDataDir);
            info.AddValue("CurentQuestionDir", curentQuestionDir);
        }
    }
}
