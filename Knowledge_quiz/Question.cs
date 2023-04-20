using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
namespace KnowledgeQuiz 
{
    [KnownType(typeof(List<string>))]
    [Serializable]
    public abstract class Question :ISerializable
    {
        [DataMember]
        protected string? question;

        [DataMember]
        private List<string>? answerVariants;

        protected Question(SerializationInfo info, StreamingContext context)
        {
            question = info.GetString("question");
            answerVariants = info.GetValue("answerVariants", typeof(List<string>)) as List<string>;
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
