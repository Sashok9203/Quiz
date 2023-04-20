using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    [Serializable]
    public class MAQuestion : Question ,ISerializable
    {
        [DataMember]
        private List<string> answers;

        protected MAQuestion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            answers = info.GetValue("answers",typeof(List<string>)) as List<string>;
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
