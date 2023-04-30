namespace KnowledgeQuiz
{

    public static class Output
    {
        public static void Write(string? text, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        public static void Write(string? text, int x , int y, ConsoleColor fColor = default, ConsoleColor bColor = default)
        {
            ConsoleColor bdef = default, fdef = default;
            if (fColor != 0)
            {
                fdef = Console.ForegroundColor;
                Console.ForegroundColor = fColor;
            }
            if (bColor != 0)
            {
                bdef = Console.BackgroundColor;
                Console.BackgroundColor = bColor;
            }
          
            Write(text,x,y);
            if (bColor != 0) Console.BackgroundColor = bdef;
            if (fColor != 0) Console.ForegroundColor = fdef;
        }
       
        public static void Write(string? text, ConsoleColor fColor)
        {
            ConsoleColor fdef = Console.ForegroundColor;
            Console.ForegroundColor = fColor;
            Console.Write(text);
            Console.ForegroundColor = fdef;
        }


        public static void WriteText(string text, int X, int Y, ConsoleColor Color)
        {
            int endIndex, startIndex = 0;
            do
            {
                endIndex = text.IndexOf('\n', startIndex);
                if (endIndex > 0) Output.Write(text[startIndex..endIndex], X, Y++, Color);
                else Output.Write(text[startIndex..], X, Y++, Color);
                startIndex = endIndex + 1;
            }
            while (startIndex != 0);
        }


        public static void  ClearRegion(int Xpos, int YPos, int XCout, int yCount)
        {
            string tmp = new string(' ', XCout);
            for (int i = 0; i != yCount; i++)
            {
                Console.SetCursorPosition(Xpos, YPos + i);
                Console.Write(tmp);
            }
        }
    }
}
