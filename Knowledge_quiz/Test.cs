using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    internal class Test 
    {
        private IEnumerable<Question> questions;
        private string name;
        private const int maxQustionCountInQuiz = 20;

        /// <summary>
        /// Метод виводить текст в заданій координаті X
        /// </summary>
        /// <param name="question"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Color"></param>
        private void printQuestion(string question, int X, int Y, ConsoleColor Color)
        {
            int endIndex, startIndex = 0;
            do
            {
                endIndex = question.IndexOf('\n', startIndex);
                if (endIndex > 0) Output.Write(question[startIndex..endIndex], X, Y++, Color);
                else Output.Write(question[startIndex..], X, Y++, Color);
                startIndex = endIndex + 1;
            }
            while (startIndex != 0);
        }


        public Test(string name, IEnumerable<Question> questions)
        {
            this.questions = questions.Take(questions.Count() < maxQustionCountInQuiz ? questions.Count() : maxQustionCountInQuiz);
            this.name = name;
        }

        /// <summary>
        /// Метод надає користувачу інтерфейс для відповіді на запитання,обрати варіант відповіді і т.д.
        /// </summary>
       
        public UserQuizInfo Start()
        {
            int testPoint = 0, count = 1;
            int X = 25, Y = 1, maxAnswers = 0, sel;
            string? title = null;
            foreach (var question in questions) 
            {
                var aVariants = Utility.Shufflet(question.AnswerVariants);
                List<string> answersVariants, answers;
                switch (question)
                {
                    case MAQuestion:
                        title = "Оберіть варіанти відповіді :";
                        maxAnswers = question.AnswerVariantsCount;
                        break;
                    case SAQuestion:
                        title = "Оберіть варіант відповіді :";
                        maxAnswers = 1;
                        break;
                }
                Console.Clear();
                Output.Write($"-= {name} =-", X, Y++, ConsoleColor.Red);
                Output.Write($"Питання {count++} / {questions.Count()}", X - 1, Y++, ConsoleColor.Gray);
                printQuestion(question.QuestionText, X - 15, Y, ConsoleColor.Green);
                answersVariants = new List<string>();
                for (int i = 0; i < question.AnswerVariantsCount; i++)
                    answersVariants.Add($"      {(char)(i + 97)}) {aVariants.ElementAt(i)}");
                Menu menu = new($"   {title}", X - 18, Console.CursorTop + 2, ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.Gray, answersVariants);
                answers = new List<string>();
                do
                {
                    sel = menu.Start();
                    if (Input.Confirm("Ви впевненні ?", "Так", "Ні", X - 12, Console.CursorTop + 2, ConsoleColor.DarkGray, ConsoleColor.Gray))
                    {
                        if (sel >= 0 && !answers.Contains(aVariants.ElementAt(sel)))
                        {
                            int y = Console.CursorTop + 2;
                            answers.Add(aVariants.ElementAt(sel));
                            maxAnswers--;
                            Output.Write($"Обрані варіанти :", X - 12, y++, ConsoleColor.Green);
                            foreach (var item in answers)
                                Output.Write($"{item}", X - 10, y++, ConsoleColor.Red);
                        }
                    }
                    else if (sel < 0) sel = 0;
                }
                while (maxAnswers > 0 && sel >= 0);
                if (question.AnswerQuestion(answers.ToArray())) testPoint++;
            }
            return new UserQuizInfo(questions.Count(), testPoint);
        }
    }
}
