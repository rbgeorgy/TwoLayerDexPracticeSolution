using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class EventsTest
    {
        private Product _testingProduct;
        private QueueWithEvents<int> _queueWithEvents;
        private NumberStream _numberStream;
        
        private void _printIfProductNameChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine("Изменилось свойство " + e.PropertyName + " продукта.Новое имя: " + sender);
        }

        private void PrintEventMessage(string message)
        {
            Console.WriteLine("Произошло событие: " + message);
        }

        [SetUp]
        public void SetUp()
        {
            _testingProduct = new Product("RTX 3060", 42, DateTime.Today, true);
            _testingProduct.PropertyChanged += _printIfProductNameChanged;
            _queueWithEvents = new QueueWithEvents<int>(3);
            _queueWithEvents.QueueOverflow += PrintEventMessage;
            _queueWithEvents.QueueUnderflow += PrintEventMessage;
            _numberStream = new NumberStream(new Collection<double>{2.0, 12.99, 321, 421, 42144443.91});
            _numberStream.TooBigDifferenceEvent += PrintEventMessage;
        }

        [Test]
        public void OnProductNamePropertyChanged()
        {
            _testingProduct.ProductName = "RTX 3070";
        }

        [Test]
        public void QueueUnderflowTest()
        {
            try
            {
                _queueWithEvents.GetItem();
            }
            catch (Exception e)
            {
                Console.WriteLine("Поймано исключение: " + e.Message);
            }
        }

        [Test]
        public void QueueOverflowTest()
        {
            for (int i = 0; i < 4; i++)
            {
                _queueWithEvents.AddItem(i);
            }
        }

        [Test]
        public void AnalyzeStreamWithConstantTest()
        {
            _numberStream.AnalyzeStreamWithConstant(1);
        }
        
        [Test]
        public void AnalyzeStreamWithCPercentageTest()
        {
            _numberStream.AnalyzeStreamWithPercentage(15);
        }
    }
}