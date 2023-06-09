﻿
using System.Runtime.Serialization;

namespace KnowledgeQuiz
{
   
    [Serializable]
    public class SAQuestion : Question
    {
        private string? answer;

        protected SAQuestion(SerializationInfo info, StreamingContext context):base(info,context)
        {
            answer = info.GetString("answer");
        }

        public SAQuestion(string questionText,  params string[] answerVariants) : base(questionText, answerVariants)
        {
            answer = null;
        }

        public override void AddAnswer(string Answer) => answer = Answer;
        
        public override bool AnswerQuestion(params string[] Answer) => (Answer?.Length) != 0 && answer == Answer?[0];

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("answer", answer);
        }

    }
}
