using System;
using System.Collections.Generic;

namespace TwoLayerSolution
{
    public class SquareFigureComparer : IComparer<Figure>
    {
        public int Compare(Figure x, Figure y)
        {
            if (x != null && y != null)
            {
                double xSquare = x.GetSquare();
                double ySquare = y.GetSquare();
                if (xSquare < ySquare) return -1;
                else if (xSquare.Equals(ySquare)) return 0;
                else return 1;
            }
            else
            {
                throw new ArgumentNullException("При сравнении фигур возникла ошибка: аргумент не может быть null.");
            }
        }
    }
}