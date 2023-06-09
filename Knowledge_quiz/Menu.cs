﻿
using System;

namespace KnowledgeQuiz
{

    public sealed class Menu
    {
        private int _selpos;
        
        private void _show()
        {
            Output.Write(Title,XPos, YPos, tColor);
            for (int i = 0; i < mItems?.Count; i++)
            {
                if (i == SelectPos) Output.Write(mItems[i].iName, XPos, YPos + i + 1, sIColor);
                else Output.Write(mItems[i].iName, XPos, YPos + i + 1, iColor);
            }
        }

        private readonly ConsoleColor tColor, iColor, sIColor;

        private readonly List<(string iName, Action? iProc)> mItems;

        public int ItemCount { get => mItems.Count; }

        public Menu(string? title, ConsoleColor titleColor,ConsoleColor iColor, ConsoleColor sIColor, params ValueTuple<string, Action?>[] items)
        {
            if (items != null ) mItems = new List<(string, Action?)>(items);
            else mItems = new List<(string, Action?)>();
            Title = title ?? "";
            XPos = 0;
            YPos = 0;
            tColor = titleColor;
            this.iColor = iColor;
            this.sIColor = sIColor;
            SelectPos = 0;
            _selpos = 0;
        }

        public Menu(string? title, ConsoleColor titleColor, ConsoleColor iColor, ConsoleColor sIColor, IEnumerable<string>? itemNames , IEnumerable<Action>? itemHandles = null) 
                   : this(title, titleColor, iColor, sIColor)
        {
            if (itemHandles != null && itemHandles.Count() != itemNames?.Count() ) throw new ApplicationException(" Кількість пунктів меню не співпадає з кількістю функцій...");
            for (int i = 0; i < itemNames?.Count(); i++)
                mItems.Add((itemNames.ElementAt(i), itemHandles?.ElementAt(i)));
        }

        public int SelectPos
        {
            get => _selpos;
            private set => _selpos = value <= 0 ? 0 : value > (mItems?.Count - 1 ?? 0) ? mItems?.Count - 1 ?? 0 : value;
        }

        public int XPos { get; set; }

        public int YPos { get; set; }

        public int Start()
        {
            ConsoleKey ck = default;
            _show();
            do
            {
                if (Console.KeyAvailable)
                {
                    ck = Console.ReadKey(true).Key;
                    if (ck == ConsoleKey.UpArrow || ck == ConsoleKey.DownArrow)
                    {
                        SelectPos -= 39 - (int)ck;
                        _show();

                    }
                    else if (ck == ConsoleKey.Enter)
                    {
                        if (mItems[SelectPos].iProc == null) return SelectPos;
                        else
                        {
                            Hide();
                            mItems[SelectPos].iProc?.Invoke();
                            _show();
                        }
                    }
                }
            }
            while (ck != ConsoleKey.Escape || mItems.Count == 0);
            return -1;
        }

        public int AddMenuItem((string, Action?) item,int index = -1)
        {
            if (index < 0) mItems.Add(item);
            else if(index < mItems.Count) mItems.Insert(index,item);
            return mItems.Count;    
        }

        public int AddMenuItem(IEnumerable<(string,Action?)> items)
        {
            mItems.AddRange(items);
            return mItems.Count;  
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
            if (index < 0 || index >= mItems.Count) return -1;
            Hide();
            mItems.RemoveAt(index);
            SelectPos = index;
            _show();
            return mItems.Count;
        }

        public void Clear()
        { 
            mItems.Clear();
            SelectPos = 0;
        }

        public void Hide()
        {
            Output.Write(new string(' ', Title?.Length ?? 0), XPos, YPos);
            for (int i = 0; i < mItems?.Count; i++)
                Output.Write(new string(' ', mItems[i].iName?.Length ?? 0),XPos, YPos + i + 1);
        }

        public string? Title { get; set; }

    }
}
