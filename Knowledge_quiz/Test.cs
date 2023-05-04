
using System.Diagnostics;


namespace KnowledgeQuiz
{
    public sealed class Test 
    {
        private IEnumerable<Question> questions;
        private readonly string? quizName;
        private readonly string? userName;
        const int maxQustionCountInQuiz = 20;


        public Test(string? userName, string? quizName, string? questionPath,SaveLoadSystem sls)
        {
          
            this.quizName = quizName;
            this.userName = userName;
           
            IEnumerable<Question> quest;
            if (quizName != Quizzes.MixedQuizName) quest = Util.Shufflet(sls.LoadQuestions(questionPath));
            else quest = Util.Shufflet(sls.AllQuestions);
            if (!quest.Any()) throw new ApplicationException($"Вікторина \"{quizName}\" не містить питань");
            int questionCount = quest.Count() < maxQustionCountInQuiz ? quest.Count() : maxQustionCountInQuiz;
            questions = quest.Take(questionCount);

        }

        /// <summary>
        /// Метод надає користувачу інтерфейс для відповіді на запитання,обрати варіант відповіді і т.д.
        /// </summary>
       
        public UserQuizInfo Start()
        {
            int testPoint = 0,
                questionNumber = 1,
                maxAnswers = 0,
                sel;
           
            var sw = new Stopwatch();
            List<string>  answers = new();
            Menu menu = new(null, ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.Gray);
            sw.Start();
            foreach (var question in questions) 
            {
                int X = 25, Y = 1;
                var aVariants = Util.Shufflet(question.AnswerVariants);
                switch (question)
                {
                    case MAQuestion:
                        menu.Title = "Оберіть варіанти відповіді :";
                        maxAnswers = question.AnswerVariantsCount;
                        break;
                    case SAQuestion:
                        menu.Title = "Оберіть варіант відповіді :";
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
                menu.AddMenuItem(aVariants.Select((n,sel )=>  ($"  {(char)(sel++ + 97)}) {n}",(Action?)null)));
                menu.XPos = X - 15;
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
