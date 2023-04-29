
namespace KnowledgeQuiz
{

    public class Menu
    {
       
        private int xPos, yPos, _selpos ;

        private int selPos
        {
            get => _selpos;
            set => _selpos = value < 0 ? 0 : value > (mItems?.Count - 1 ?? 0)? mItems?.Count - 1 ?? 0 : value;
        }

        private void clear()
        {
            Output.Write(new string(' ',Title?.Length ?? 0), (int)xPos, (int)yPos);
            for (int i = 0; i < mItems?.Count; i++)
                 Output.Write(new string(' ', mItems?[i].iName?.Length ?? 0), (int)xPos, (int)yPos + i + 1);
        }

        private void show()
        {
            Output.Write(Title, (int)xPos, (int)yPos, nColor);
            for (int i = 0; i < mItems?.Count; i++)
            {
                if (i == selPos) Output.Write(mItems[i].iName, (int)xPos, (int)yPos + i + 1, sIColor);
                else Output.Write(mItems[i].Item1, (int)xPos, (int)yPos + i + 1, iColor);
            }
        }

        private readonly ConsoleColor nColor, iColor, sIColor;

        public string? Title { get; set; }

        private  List<(string? iName, Func<bool>? iProc)> mItems;

        public int ItemCount { get => mItems?.Count ?? 0; }

        public Menu(string? title,int X,int Y, ConsoleColor titleColor,ConsoleColor iColor, ConsoleColor sIColor, params ValueTuple<string, Func<bool>>[] items)
        {
            if (items != null ) mItems = new List<(string?, Func<bool>?)>(items);
            else mItems = new List<(string?, Func<bool>?)>();
            Title = title ?? "";
            xPos = X;
            yPos = Y;
            this.nColor = titleColor;
            this.iColor = iColor;
            this.sIColor = sIColor;
            _selpos = 0;
        }

        public Menu(string? title, int X, int Y, ConsoleColor titleColor, ConsoleColor iColor, ConsoleColor sIColor, IEnumerable<string>? itemNames, IEnumerable<Func<bool>>? itemHandles = null) 
                   : this(title, X, Y, titleColor, iColor, sIColor)
        {
            if (itemHandles != null && itemHandles.Count() != itemNames?.Count() ) throw new ApplicationException(" Кількість пунктів меню не співпадає з кількістю функцій...");
            for (int i = 0; i < itemNames?.Count(); i++)
                mItems.Add((itemNames.ElementAt(i), itemHandles?.ElementAt(i)));
        }

        public int Start()
        {
            ConsoleKey ck = default;
            show();
            do
            {
                if (Console.KeyAvailable)
                {
                    ck = Console.ReadKey(true).Key;
                    if (ck == ConsoleKey.UpArrow || ck == ConsoleKey.DownArrow) selPos -= 39 - (int)ck;
                    else if (ck == ConsoleKey.Enter)
                    {
                       if(mItems[selPos].iProc?.Invoke() ?? true) return selPos;
                        Console.Clear();
                    }
                    show();
                }
            }
            while (ck != ConsoleKey.Escape || mItems.Count == 0);
            return -1;
        }

        public int AddMenuItem((string, Func<bool>?) item,int index = -1)
        {
            if (index < 0) mItems?.Add(item);
            else if(index < mItems?.Count) mItems.Insert(index,item);
            show();
            return mItems?.Count ?? 0;    
        }

        public int DelMenuItem(int index)
        {
            if (index < 0 || index >= mItems?.Count) return -1;
            clear();
            mItems?.RemoveAt(index);
            selPos = 0;
            show();
            return mItems?.Count ?? 0;
        }
    }
}
