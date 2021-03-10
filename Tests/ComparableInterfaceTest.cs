using System;
using NUnit.Framework;
using TwoLayerSolution;
using TwoLayerSolution.Comparers;


namespace Tests
{
    [TestFixture]
    public class ComparableInterfaceTest
    {
        private enum SortParameters { Square, Perimeter }

        private Figure[] Generate()
        {
            var rand = new Random();
            Figure[] figureArray = new Figure[100];
            for (int i = 0; i < figureArray.Length; i++)
            {
                switch (i % 3)
                {
                    case 0:
                        figureArray[i] = new Circle(rand.Next(1, 10));
                        break;
                    case 1:
                        figureArray[i] = new Rectangle(rand.Next(1, 5), rand.Next(1, 5));
                        break;
                    case 2:
                        int aSide = rand.Next(5, 10);
                        int bSide = rand.Next(5, 10);
                        int cSide = (int) ((aSide + bSide) * 0.5);
                        figureArray[i] = new Triangle(aSide, bSide, cSide);
                        break;
                }
            }
            return figureArray;
        }

        private Figure[] SortBySquare()
        {
            Figure[] figureArray = Generate();
            Array.Sort(figureArray);
            return figureArray;
        }

        private Figure[] SortBySquareWithExternalComparer()
        {
            Figure[] figureArray = Generate();
            Array.Sort(figureArray, new SquareFigureComparer());
            return figureArray;
        }
        
        private Figure[] SortByPerimeterWithExternalComparer()
        {
            Figure[] figureArray = Generate();
            Array.Sort(figureArray, new PerimeterFigureComparer());
            return figureArray;
        }

        private void PrintArray(string parameter, Figure[] figureArray)
        {
            if (parameter == null) throw new ArgumentException("Нельзя печатать без параметра");
            else
            {
                foreach (var item in figureArray)
                {
                    double toPrint = parameter=="с площадью" ? item.GetSquare() : item.GetPerimeter();
                    Console.WriteLine(item + ", " + parameter + ": " + toPrint);
                }
                Console.WriteLine();
            }
        }
        
        private bool IsSorted(Figure[] array, SortParameters parameters)
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
            Generate();
        }

        [Test]
        public void SortBySquareTest1()
        {
            Figure[] figureArray = SortBySquare();
            PrintArray("с площадью", figureArray);
            Assert.True(IsSorted(figureArray, SortParameters.Square));
        }
        
        [Test]
        public void SortBySquareWithExternalComparerTest()
        {
            Figure[] figureArray = SortBySquareWithExternalComparer();
            PrintArray("с площадью", figureArray);
            Assert.True(IsSorted(figureArray, SortParameters.Square));
        }
        
        [Test]
        public void SortByPerimeterWithExternalComparerTest()
        {
            Figure[] figureArray = SortByPerimeterWithExternalComparer();
            PrintArray("с периметром", figureArray);
            Assert.True(IsSorted(figureArray, SortParameters.Perimeter));
        }
    }
}