using System;
using System.Collections.Generic;

namespace TwoLayerSolution
{
    public class PerimeterFigureComparer : IComparer<Figure>
    {
        public int Compare(Figure x, Figure y)
        {
            if (x != null && y != null)
            {
                double xPerimeter = x.GetPerimeter();
                double yPerimeter = y.GetPerimeter();
                if (xPerimeter < yPerimeter) return -1;
                else if (xPerimeter.Equals(yPerimeter)) return 0;
                else return 1;
            }
            else
            {
                throw new ArgumentNullException("При сравнении фигур возникла ошибка: аргумент не может быть null.");
            }
        }
    }
}