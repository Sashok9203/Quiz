
using System.Runtime.Serialization;


namespace KnowledgeQuiz
{
    [Serializable]
    public class MAQuestion : Question 
    {
        
        private List<string> answers;

        protected MAQuestion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            answers = info.GetValue("answers",typeof(List<string>)) as List<string> ?? new();
        }
        public MAQuestion(string questionText,params string[] answerVariants) : base(questionText,answerVariants)
        {
            answers = new List<string>();
        }

        public override void AddAnswer(string Answer)
        {
            if (!string.IsNullOrWhiteSpace(Answer)) answers.Add(Answer);
        }

        public override bool AnswerQuestion(params string[] Answer)
        {
            if (Answer.Length != answers.Count) return false;
            for (int i = 0; i < Answer.Length; i++)
            {
                bool found = false;
                foreach (string ans in answers) 
                {
                    if (ans == Answer[i])
                    {
                        found = true;
                        break;
                    }
                }
                if(!found) return false;
            }
            return true;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("answers", answers);
        }

    }
}
