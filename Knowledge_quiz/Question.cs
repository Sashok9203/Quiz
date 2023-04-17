using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    internal abstract class Question
    {
		protected string _question;

		public string QuestionString
        {
			get  => _question;
			set 
			{
				if (string.IsNullOrWhiteSpace(value)) throw new ApplicationException("Текст питання не може бути пустим...");
				_question = value;
			}
		}

		protected Question() { }
    }
}
