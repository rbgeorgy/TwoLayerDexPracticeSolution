using System;
using NUnit.Framework;
using TwoLayerSolution;


namespace Tests
{
    [TestFixture]
    public class ComparableInterfaceTest
    {
        private Figure[] _figureArray;

        private enum SortParameters { Square, Perimeter }

        private void _generate()
        {
            var rand = new Random();
            _figureArray = new Figure[100];
            for (int i = 0; i < _figureArray.Length; i++)
            {
                switch (i % 3)
                {
                    case 0:
                        _figureArray[i] = new Circle(rand.Next(1, 10));
                        break;
                    case 1:
                        _figureArray[i] = new Rectangle(rand.Next(1, 5), rand.Next(1, 5));
                        break;
                    case 2:
                        int aSide = rand.Next(5, 10);
                        int bSide = rand.Next(5, 10);
                        int cSide = (int) ((aSide + bSide) * 0.5);
                        _figureArray[i] = new Triangle(aSide, bSide, cSide);
                        break;
                }
            }
        }

        private void _sortBySquare()
        {
            Array.Sort(_figureArray);
        }

        private void _sortBySquareWithExternalComparer()
        {
            Array.Sort(_figureArray, new SquareFigureComparer());
        }
        
        private void _sortByPerimeterWithExternalComparer()
        {
            Array.Sort(_figureArray, new PerimeterFigureComparer());
        }

        private void _printArray(string parameter)
        {
            if (parameter == null) throw new ArgumentException("Нельзя печатать без параметра");
            else
            {
                foreach (var item in _figureArray)
                {
                    Console.WriteLine(item + ", " + parameter + ": " + item.GetSquare());
                }
                Console.WriteLine();
            }
        }
        
        private bool _isSorted(Figure[] array, SortParameters parameters)
        {
            if (array == null) throw new ArgumentException("Передайте существующий массив для проверки.");
            for (int i = 1; i < array.Length; i++)
            {
                if (parameters == SortParameters.Square
                    ? (array[i - 1].GetSquare() > array[i].GetSquare())
                    : (array[i - 1].GetPerimeter() > array[i].GetPerimeter()))
                {
                    return false;
                }
            }
            return true;
        }

        [SetUp]
        public void SetUp()
        {
            _generate();
        }

        [Test]
        public void SortBySquareTest1()
        {
            _sortBySquare();
            _printArray("с площадью");
            Assert.True(_isSorted(_figureArray, SortParameters.Square));
        }
        
        [Test]
        public void SortBySquareBySquareWithExternalComparerTest()
        {
            _sortBySquareWithExternalComparer();
            _printArray("с площадью");
            Assert.True(_isSorted(_figureArray, SortParameters.Square));
        }
        
        [Test]
        public void SortByPerimeterBySquareWithExternalComparerTest()
        {
            _sortByPerimeterWithExternalComparer();
            _printArray("с периметром");
            Assert.True(_isSorted(_figureArray, SortParameters.Perimeter));
        }
    }
}