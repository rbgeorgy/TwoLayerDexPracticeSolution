using System;

namespace TwoLayerSolution
{
    public abstract class Figure : System.IComparable
    {
        public abstract double GetSquare();
        public abstract double GetPerimeter();
        
        private const double Precision = 0.00001;
        public int CompareTo(object other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("Данной фигуры не существует.");
            }
            else
            {
                if (other is Figure)
                {
                    double othersSquare = ((Figure) other).GetSquare();
                    double thisSquare = this.GetSquare();
                    if (thisSquare < othersSquare) return -1;
                    else if (thisSquare - othersSquare < Precision) return 0;
                    else return 1;
                }
                else throw new ArgumentException("Сравнивать можно только с фигурой.");
            }
        }
    }
}