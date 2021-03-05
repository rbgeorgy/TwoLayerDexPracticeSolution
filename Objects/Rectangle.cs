using System;

namespace TwoLayerSolution
{
    public class Rectangle : Figure
    {
        private readonly double _aSide;
        private readonly double _bSide;

        public Rectangle(double aSide, double bSide)
        {
            if (aSide > 0 && bSide > 0)
            {
                _aSide = aSide;
                _bSide = bSide;
            }
            else
            {
                throw new ArgumentException("Стороны прямоугольника не могут быть нулевой или отрицательной длины.");
            }
        }

        public override double GetSquare()
        {
            return _aSide * _bSide;
        }

        public override double GetPerimeter()
        {
            return  2 * (_aSide + _bSide);
        }

        public override string ToString()
        {
            return "Прямоугольник";
        }
    }
}