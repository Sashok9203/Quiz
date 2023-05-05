
using System.Runtime.Serialization;


namespace KnowledgeQuiz
{
    
    [Serializable]
    public sealed class Setting : ISerializable
    {
        public  string? curentDataDir;
        public  string? curentQuestionDir;

        public Setting()
        {
            curentDataDir = null;
            curentQuestionDir = null;
        }

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
