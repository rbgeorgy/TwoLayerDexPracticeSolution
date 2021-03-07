using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace TwoLayerSolution
{
    public class NumberStream
    {
        public delegate void NumberDifference(string message);

        public event NumberDifference TooBigDifferenceEvent;

        private readonly Collection<double> _numberStream;
        private double _averageValue;

        public NumberStream(Collection<double> numberStream)
        {
            _numberStream = numberStream ?? throw new ArgumentNullException("Передайте в конструктор потока чисел существующую коллекцию");
            _calculateAverageValue();
        }

        private void _calculateAverageValue()
        {
            double sum = 0;
            foreach (var item in _numberStream)
            {
                sum += item;
            }
            sum /= _numberStream.Count;
            _averageValue = sum;
        }

        public void AnalyzeStreamWithConstant(double difference)
        {
            if (difference < 0) throw new ArgumentException("Разница должна быть положительной");
            double previous = _numberStream.First();
            foreach (var item in _numberStream)
            {
                if (Math.Abs(item - previous) > difference)
                {
                    TooBigDifferenceEvent?.Invoke("Элементы " + previous + " и " + item + " различаются слишком сильно.\nДопустимая разница: " + difference + " Обнаруженная: " + Math.Abs(item - previous));
                    previous = item;
                }
                else
                {
                    previous = item;
                }
            }
        }

        public void AnalyzeStreamWithPercentage(int percentage)
        {
            if (percentage < 0) throw new ArgumentException("Процент от 0");
            foreach (var item in _numberStream)
            {
                if (100 * item / _averageValue > percentage)
                {
                    TooBigDifferenceEvent?.Invoke("Элемент " + item + " отличается от среднего значения " +_averageValue +" слишком сильно.\nДопустимая разница в процентах: " + percentage + "% Обнаруженная: " + 100 * item / _averageValue+"%");
                }
            }
        }
    }
}