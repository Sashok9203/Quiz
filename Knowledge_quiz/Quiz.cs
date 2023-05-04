
using System.Text;
using System.Runtime.Serialization;
using System.Reflection.PortableExecutable;


namespace KnowledgeQuiz
{


    [KnownType(typeof(List<Question>))]
    internal partial class Quiz 
    {
       
        private const string passwordRegex = @"[^ \p{IsCyrillic}]";

        private const string loginRegex = @"[0-9a-zA-Z_]";

        private const int passwordMaxLenght = 12;

        public const int loginMaxLenght = 12;

        private Quizzes quizzes => SLSystem.Quizzes;

        private Users users => SLSystem.Users;

        private Rating rating => SLSystem.Rating;

        private readonly SaveLoadSystem SLSystem;
     
        public Quiz()
        {
            SLSystem = new();
        }

        public void Start()
        {

            Menu startMenu = new("   -= Вікторина знань =-", ConsoleColor.Green, ConsoleColor.DarkGray, ConsoleColor.Gray,
                ("          Увійти",  () =>  Enter()),
                ("        Реєстрація",  () => Registration()),
                ("     Адмініструввання",  () => { StartUtility("EditUtility.exe") ; Console.Clear(); }
            ));
            startMenu.XPos = 10;
            startMenu.YPos = 1;
            startMenu.Start();
            Console.Clear();
        }
    }
}
