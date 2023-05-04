
using System.Runtime.Serialization;


namespace KnowledgeQuiz
{
    [Serializable]
    public class MAQuestion : Question 
    {
        
        private readonly List<string> answers;

        protected MAQuestion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            answers = info.GetValue("answers",typeof(List<string>)) as List<string> ?? new();
        }
        public MAQuestion(string questionText,params string[] answerVariants) : base(questionText,answerVariants)
        {
            answers = new ();
        }

        public override void AddAnswer(string Answer)
        {
            if (!string.IsNullOrWhiteSpace(Answer)) answers.Add(Answer);
        }

        public override bool AnswerQuestion(params string[] Answer)
        {
            if (Answer.Length != answers.Count) return false;
            return !answers.Except(Answer).Any();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("answers", answers);
        }

    }
}
