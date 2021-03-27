using System;
using TwoLayerSolution.FigureInterfaces;

namespace TwoLayerSolution
{
    public class PrintFigureMethodsResults<T> : IPrintFigureMethodsResults<T> where T : Figure
    {
        public delegate void Notifier(string message);
        public event Notifier Notify;
        
        public void PrintToConsoleFigureMethodsResults(T figure)
        {
            var message = "Площадь фигуры: " + figure.GetSquare() + "\nПериметр фигуры: " + figure.GetPerimeter();
            if (Notify == null)
                Notify += PrintMessage;
            
            Notify?.Invoke(message);
        }
        
        private void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}