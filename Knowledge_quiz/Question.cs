using System.Runtime.Serialization;

namespace KnowledgeQuiz 
{
  
    [KnownType(typeof(SAQuestion))]
    [KnownType(typeof(MAQuestion))]
    [KnownType(typeof(List<string>))]
    [Serializable]
    public abstract class Question :ISerializable
    {
        protected string? question;
       
        private readonly List<string> answerVariants;

        protected Question(SerializationInfo info, StreamingContext context)
        {
            question = info.GetString("question");
            answerVariants = info.GetValue("answerVariants", typeof(List<string>)) as List<string> ?? new();
        }

        protected Question(string? questionText, params string[]? AnswerVariants)
        {

            answerVariants = AnswerVariants != null ? new List<string>(AnswerVariants) :  new List<string>();
            QuestionText = questionText ??= "InvalidName";
        }

        public void ClearAnswerVariants() => answerVariants?.Clear();

        public string QuestionText
        {
            get => question ??= "";
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ApplicationException("Текст питання не може бути пустим...");
                question = value;
            }
        }

        public int AnswerVariantsCount => answerVariants?.Count ?? 0;

        public void AddAnswerVariant(string AnswerVariant)
        {
            if (!string.IsNullOrWhiteSpace(AnswerVariant)) answerVariants.Add(AnswerVariant);
        }

        public void RemoveAnswerVariant(int index)
        {
            if (index >= 0 && index < answerVariants.Count) answerVariants.RemoveAt(index);
        }

        public IEnumerable<string> AnswerVariants => answerVariants;

        public abstract void AddAnswer(string answer);

        public abstract bool AnswerQuestion(params string[] answer);

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("question", question);
            info.AddValue("answerVariants", answerVariants);
        }
    }
}
