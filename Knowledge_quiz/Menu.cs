using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Output.WriteLine(new string(' ',Title.Length), (int)xPos, (int)yPos);
            for (int i = 0; i < mItems?.Count; i++)
                 Output.WriteLine(new string(' ', mItems[i].iName.Length), (int)xPos, (int)yPos + i + 1);
        }

        private void show()
        {
            Output.WriteLine(Title, (int)xPos, (int)yPos, nColor);
            for (int i = 0; i < mItems?.Count; i++)
            {
                if (i == selPos) Output.WriteLine(mItems[i].Item1, (int)xPos, (int)yPos + i + 2, sIColor);
                else Output.WriteLine(mItems[i].Item1, (int)xPos, (int)yPos + i + 2, iColor);
            }
        }

        private readonly ConsoleColor nColor, iColor, sIColor;

        public string Title { get; set; }

        private  List<(string iName, Action iProc)>? mItems;

        public int ItemCount { get => mItems?.Count ?? 0; }

        private Menu(string? title, int X, int Y, ConsoleColor titleColor, ConsoleColor iColor, ConsoleColor sIColor)
        {
            Title = title ?? "";
            xPos = X;
            yPos = Y;
            this.nColor = titleColor;
            this.iColor = iColor;
            this.sIColor = sIColor;
            _selpos = 0;
        }

        public Menu(string title,int X,int Y, ConsoleColor titleColor,ConsoleColor iColor, ConsoleColor sIColor, params ValueTuple<string,Action> [] items)
            :this( title,  X,  Y,  titleColor,  iColor,  sIColor) { mItems = new List<(string, Action)>(items);}

        public Menu(string title, int X, int Y, ConsoleColor titleColor, ConsoleColor iColor, ConsoleColor sIColor, string[] itemNames, Action[] itemHandles) : this(title, X, Y, titleColor, iColor, sIColor)
        {
            if (itemNames.Length != itemHandles.Length) throw new ApplicationException(" Кількість пунктів меню не співпадає з кількістю функцій...");
            mItems = new List<(string, Action)>();
            for (int i = 0; i < itemNames.Length; i++)
                mItems.Add((itemNames[i], itemHandles[i]));
        }

        public void  Start()
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
                        mItems?[selPos].iProc.Invoke();
                        Console.Clear();
                    }
                    show();
                }
            }
            while (ck != ConsoleKey.Escape || mItems?.Count == 0);
        }

        public int AddMenuItem((string, Action) item,int index = -1)
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
            show();
            return mItems?.Count ?? 0;
        }
    }
}
