using System;
using NUnit.Framework;
using TwoLayerSolution;
using TwoLayerSolution.FigureInterfaces;

namespace Tests
{
    public class CovarianсeCounterCovarianсeTest
    {
        [Test]
        public void CircleCollectionReverserCovarianceTest()
        {
            Assert.DoesNotThrow(() =>
            {
                IFigureCollectionReverser<Figure> circleCollectionReverser = new CircleCollectionReverser<Circle>(new []
                {
                    new Circle(4),
                    new Circle(15)
                });
                circleCollectionReverser.ReverseAndThrowEvent();
            });
        }

        [Test]
        public void PrintFigureMethodsResultCounterCovarianceTest()
        {
            Assert.DoesNotThrow(() =>
            {
                IPrintFigureMethodsResults<Triangle> printFigureMethodsResults =
                    new PrintFigureMethodsResults<Figure>();
                printFigureMethodsResults.PrintToConsoleFigureMethodsResults(new Triangle(3, 4, 5)); ;
            });
        }
    }
}