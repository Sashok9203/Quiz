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

        private int xPos, yPos, _selpos = 0;

        private int selPos
        {
            get => _selpos;
            set => _selpos = value < 0 ? 0 : value > mItems.Length - 1 ? mItems.Length - 1 : value;
        }

        ConsoleColor nColor, iColor, sIColor;

        public string Title { get; set; }

        private readonly ValueTuple<string, MenuEventHandler>[] mItems;

        public int ItemCount { get => mItems?.Length ?? 0; }

        private void _show()    
        {
            Output.WriteLine(Title, (int) xPos, (int) yPos, nColor);
            for (int i = 0; i<mItems.Length; i++)
            {
                if (i == selPos) Output.WriteLine(mItems[i].Item1, (int) xPos, (int)yPos + i + 1, sIColor);
                else Output.WriteLine(mItems[i].Item1, (int) xPos, (int)yPos + i + 1, iColor);
            }
        }

        public Menu(string name,int X,int Y, ConsoleColor nColor,ConsoleColor iColor, ConsoleColor sIColor, params ValueTuple<string, MenuEventHandler> [] items)
        {
            Title = name;
            mItems = items;
            xPos = X;
            yPos = Y;
            this.nColor = nColor;
            this.iColor = iColor;
            this.sIColor = sIColor;
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
