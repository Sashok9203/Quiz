using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{

    internal class Menu
    {
        public delegate void MenuEventHandler();

        private int xPos, yPos, _selpos ;

        private int selPos
        {
            get => _selpos;
            set => _selpos = value < 0 ? 0 : value > mItems.Length - 1 ? mItems.Length - 1 : value;
        }

        private void _show()
        {
            Output.WriteLine(Title, (int)xPos, (int)yPos, nColor);
            for (int i = 0; i < mItems.Length; i++)
            {
                if (i == selPos) Output.WriteLine(mItems[i].Item1, (int)xPos, (int)yPos + i + 1, sIColor);
                else Output.WriteLine(mItems[i].Item1, (int)xPos, (int)yPos + i + 1, iColor);
            }
        }

        private readonly ConsoleColor nColor, iColor, sIColor;

        public string Title { get;}

        private readonly ValueTuple<string, MenuEventHandler>[] mItems;

        public int ItemCount { get => mItems?.Length ?? 0; }

        private Menu(string title, int X, int Y, ConsoleColor titleColor, ConsoleColor iColor, ConsoleColor sIColor)
        {
            Title = title;
            xPos = X;
            yPos = Y;
            this.nColor = titleColor;
            this.iColor = iColor;
            this.sIColor = sIColor;
            _selpos = 0;
        }

        public Menu(string title,int X,int Y, ConsoleColor titleColor,ConsoleColor iColor, ConsoleColor sIColor, params ValueTuple<string, MenuEventHandler> [] items)
            :this( title,  X,  Y,  titleColor,  iColor,  sIColor) { mItems = items;}

        public Menu(string title, int X, int Y, ConsoleColor titleColor, ConsoleColor iColor, ConsoleColor sIColor, string[] itemNames, MenuEventHandler[] itemHandles) : this(title, X, Y, titleColor, iColor, sIColor)
        {
            if (itemNames.Length != itemHandles.Length) throw new ApplicationException(" Кількість пунктів меню не співпадає з кількістю функцій...");
            mItems = new (string, MenuEventHandler)[itemNames.Length];
            for (int i = 0; i < itemNames.Length; i++)
                mItems[i] = (itemNames[i], itemHandles[i]);
        }

        public void  Start()
        {
            ConsoleKey ck = default;
            _show();
            do
            {
                if (Console.KeyAvailable)
                {
                    ck = Console.ReadKey(true).Key;
                    if (ck == ConsoleKey.UpArrow || ck == ConsoleKey.DownArrow) selPos -= 39 - (int)ck;
                    else if (ck == ConsoleKey.Enter) mItems[selPos].Item2.Invoke();
                    _show();
                }
            }
            while (ck != ConsoleKey.Escape);
        }
    }
}
