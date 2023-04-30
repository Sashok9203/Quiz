
namespace KnowledgeQuiz
{

    public class Menu
    {
        private int _selpos;
       
        private void clear()
        {
            Output.Write(new string(' ',Title?.Length ?? 0), (int)XPos, (int)YPos);
            for (int i = 0; i < mItems?.Count; i++)
                 Output.Write(new string(' ', mItems?[i].iName?.Length ?? 0), (int)XPos, (int)YPos + i + 1);
        }

        private void show()
        {
            Output.Write(Title,XPos, YPos, tColor);
            for (int i = 0; i < mItems?.Count; i++)
            {
                if (i == SelectPos) Output.Write(mItems[i].iName, XPos, YPos + i + 1, sIColor);
                else Output.Write(mItems[i].iName, XPos, YPos + i + 1, iColor);
            }
        }

        private readonly ConsoleColor tColor, iColor, sIColor;

        private readonly List<(string iName, Func<bool>? iProc)> mItems;

        public int ItemCount { get => mItems?.Count ?? 0; }

        public Menu(string? title, ConsoleColor titleColor,ConsoleColor iColor, ConsoleColor sIColor, params ValueTuple<string, Func<bool>?>[] items)
        {
            if (items != null ) mItems = new List<(string, Func<bool>?)>(items);
            else mItems = new List<(string, Func<bool>?)>();
            Title = title ?? "";
            XPos = 0;
            YPos = 0;
            this.tColor = titleColor;
            this.iColor = iColor;
            this.sIColor = sIColor;
            _selpos = 0;
        }

        public Menu(string? title, ConsoleColor titleColor, ConsoleColor iColor, ConsoleColor sIColor, IEnumerable<string>? itemNames , IEnumerable<Func<bool>>? itemHandles = null) 
                   : this(title, titleColor, iColor, sIColor)
        {
            if (itemHandles != null && itemHandles.Count() != itemNames?.Count() ) throw new ApplicationException(" Кількість пунктів меню не співпадає з кількістю функцій...");
            for (int i = 0; i < itemNames?.Count(); i++)
                mItems.Add((itemNames.ElementAt(i), itemHandles?.ElementAt(i)));
        }

        public int SelectPos
        {
            get => _selpos;
            set => _selpos = value < 0 ? 0 : value > (mItems?.Count - 1 ?? 0) ? mItems?.Count - 1 ?? 0 : value;
        }

        public int XPos { get; set; }

        public int YPos { get; set; }

        public int Start()
        {
            ConsoleKey ck = default;
            show();
            do
            {
                if (Console.KeyAvailable)
                {
                    ck = Console.ReadKey(true).Key;
                    if (ck == ConsoleKey.UpArrow || ck == ConsoleKey.DownArrow) SelectPos -= 39 - (int)ck;
                    else if (ck == ConsoleKey.Enter)
                    {
                       if(mItems[SelectPos].iProc?.Invoke() ?? true) return SelectPos;
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
            return mItems?.Count ?? 0;    
        }

        public string GetItemString(int ind) => mItems[ind].iName;

        public void SetItemString(int ind, string str)
        {
            var tmp = mItems[ind];
            tmp.iName = str;
            mItems[ind] = tmp;
        }

        public int  DelMenuItem(int index)
        {
            if (index < 0 || index >= mItems?.Count) return -1;
            clear();
            mItems?.RemoveAt(index);
            SelectPos = 0;
            show();
            return mItems?.Count ?? 0;
        }

        public void Clear() => mItems.Clear();

        public string? Title { get; set; }

    }
}
