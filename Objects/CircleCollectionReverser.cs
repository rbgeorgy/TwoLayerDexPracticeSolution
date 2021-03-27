using System;
using System.Collections.Generic;
using System.Linq;
using TwoLayerSolution.FigureInterfaces;

namespace TwoLayerSolution
{
    public class CircleCollectionReverser<T> : IFigureCollectionReverser<T> where T : Figure
    {
        private readonly IEnumerable<T> _circleArray;

        public delegate void Notifier(string message);
        public event Notifier Notify;
        
        public CircleCollectionReverser(IEnumerable<T> circleArray)
        {
            _circleArray = circleArray ?? throw new ArgumentNullException(nameof(circleArray));
            Notify += PrintMessage;
        }

        public IEnumerable<T> ReverseAndThrowEvent()
        {
            Notify?.Invoke("Массив окружностей был перевёрнут");
            return _circleArray.Reverse();
        }
        
        private void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }
        
    }
}