using System;

namespace TwoLayerSolution
{
    public class Triangle : Figure
    {
        private readonly double _aSide;
        private readonly double _bSide;
        private readonly double _cSide;

        public Triangle(double aSide, double bSide, double cSide)
        {
            if (aSide > 0 && bSide > 0 && cSide > 0)
            {
                if (aSide + bSide > cSide && bSide + cSide > aSide && aSide + cSide > bSide)
                {
                    _aSide = aSide;
                    _bSide = bSide;
                    _cSide = cSide;   
                }
                else
                {
                    throw new ArgumentException("Треугольник с заданными сторонами не существует.");
                }
            }
            else
            {
                throw new ArgumentException("Стороны не могуты быть нулевыми или отрицательными.");
            }
        }

        public override double GetSquare()
        {
            double halfPerimeter = (_aSide + _bSide + _cSide) / 2.0;
            return Math.Sqrt(halfPerimeter*(halfPerimeter-_aSide)*(halfPerimeter-_bSide)*(halfPerimeter-_cSide));
        }
        
        public override double GetPerimeter()
        {
            return _aSide + _bSide + _cSide;
        }
        
        public override string ToString()
        {
            return "Треугольник";
        }
    }
}