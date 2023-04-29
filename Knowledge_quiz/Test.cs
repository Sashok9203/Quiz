
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
            int testPoint = 0, count = 1;
            int  maxAnswers = 0, sel;
            string? title = null;
            var sw = new Stopwatch();
            sw.Start();
            foreach (var question in questions) 
            {
                int X = 25, Y = 1;
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
                Output.Write($"-= {quizName} =-", X, Y++, ConsoleColor.Red);
                Output.Write($"Питання {count++} / {questions.Count()}", X - 1, Y++, ConsoleColor.Gray);
                Output.WriteText(question.QuestionText, X - 15, Y, ConsoleColor.Green);
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
            sw.Stop();
            return new UserQuizInfo(userName,quizName, questions.Count(), testPoint,sw.ElapsedTicks);
        }
    }
}
