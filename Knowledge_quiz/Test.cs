
using System.Diagnostics;


namespace KnowledgeQuiz
{
    public class Test 
    {
        private IEnumerable<Question> questions;
        private readonly string? quizName;
        private readonly string? userName;


        public Test(string? userName, string? quizName, IEnumerable<Question> questions)
        {
            this.questions = questions;
            this.quizName = quizName;
            this.userName = userName;
        }

        /// <summary>
        /// Метод надає користувачу інтерфейс для відповіді на запитання,обрати варіант відповіді і т.д.
        /// </summary>
       
        public UserQuizInfo Start()
        {
            int testPoint = 0,
                questionNumber = 1,
                maxAnswers = 0,
                sel = 0;
            string? title = null;
            var sw = new Stopwatch();
            List<string>  answers = new();
            Menu menu = new($"   {title}", ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.Gray);
            sw.Start();
            foreach (var question in questions) 
            {
                int X = 25, Y = 1;
                var aVariants = Utility.Shufflet(question.AnswerVariants);
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
                Output.Write($"-= {quizName} =-", X, Y++, ConsoleColor.Red);
                Output.Write($"Питання {questionNumber++} / {questions.Count()}", X - 1, Y++, ConsoleColor.Gray);
                Output.WriteText(question.QuestionText, X - 15, Y, ConsoleColor.Green);
                menu.Clear();
                answers.Clear();
                sel = 0;
                foreach (var item in aVariants)
                    menu.AddMenuItem(($"  {(char)(sel++ + 97)}) {item}", null));
                menu.XPos = X - 18;
                menu.YPos = Console.CursorTop + 2;
                do
                {
                    sel = menu.Start();
                    if (sel >= 0 )
                    {
                        if (!answers.Contains(aVariants.ElementAt(sel)))
                        {
                            maxAnswers--;
                            if (maxAnswers == 0)
                            {
                                if (Input.Confirm("Ви впевненні ?", "Так", "Ні", X - 12, Console.CursorTop + 2, ConsoleColor.DarkGray, ConsoleColor.Gray))
                                {
                                    answers.Add(aVariants.ElementAt(sel));
                                    sel = -1;
                                }
                                else maxAnswers++;
                            }
                            else
                            {
                                answers.Add(aVariants.ElementAt(sel));
                                menu.SetItemString(sel, "*" + menu.GetItemString(sel)[1..]);
                            }
                        }
                        else
                        {
                            answers.Remove(aVariants.ElementAt(sel));
                            maxAnswers++;
                            menu.SetItemString(sel, " " + menu.GetItemString(sel)[1..]);
                        }
                    }
                    else if (!Input.Confirm("Ви впевненні ?", "Так", "Ні", X - 12, Console.CursorTop + 2, ConsoleColor.DarkGray, ConsoleColor.Gray)) sel = 0;
                }
                while (sel >= 0);
                if (question.AnswerQuestion(answers.ToArray())) testPoint++;
            }
            sw.Stop();
            return new UserQuizInfo(userName,quizName, questions.Count(), testPoint,sw.ElapsedTicks);
        }
    }
}
