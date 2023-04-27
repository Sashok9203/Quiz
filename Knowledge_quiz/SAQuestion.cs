using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KnowledgeQuiz
{
   
    [Serializable]
    public class SAQuestion : Question, ISerializable
    {
        private string? answer;

        protected SAQuestion(SerializationInfo info, StreamingContext context):base(info,context)
        {
            answer = info.GetString("answer");
        }

        public SAQuestion(string questionText,  params string[] answerVariants) : base(questionText, answerVariants)
        {
            answer = "";
        }

        public override void AddAnswer(string Answer)
        {
            if (!string.IsNullOrWhiteSpace(Answer)) answer = Answer;
        }

        public override bool AnswerQuestion(params string[] Answer) => (Answer?.Length) != 0 && answer == Answer?[0];

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("answer", answer);
        }

    }
}
