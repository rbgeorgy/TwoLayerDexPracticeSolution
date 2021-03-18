using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class EventsTest
    {
        [Test]
        public void OnProductNamePropertyChanged()
        {
            var testingProduct = new Product("RTX 3060", 42, DateTime.Today, true);
            string propertyName = null;
            testingProduct.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
            {
                propertyName = e.PropertyName;
            };
            testingProduct.ProductName = "RTX 3040";
            Assert.IsNotNull(propertyName);
            Assert.AreEqual("ProductName", propertyName);
        }

        [Test]
        public void QueueUnderflowTest()
        {
            QueueWithEvents<int> queueWithEvents = new QueueWithEvents<int>(1);
            string underflowMessage = null;
            queueWithEvents.QueueUnderflow += delegate(string notify)
            {
                underflowMessage = notify;
            };
            try
            {
                queueWithEvents.GetItem();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Assert.IsNotNull(underflowMessage);
                Assert.AreEqual("Очередь пуста!", underflowMessage);   
            }
        }

        [Test]
        public void QueueOverflowTest()
        {
            QueueWithEvents<int> queueWithEvents = new QueueWithEvents<int>(3);
            string overflowMessage = null;
            queueWithEvents.QueueOverflow += delegate(string notify)
            {
                overflowMessage = notify;
            };
            for (int i = 0; i < 4; i++)
            {
                queueWithEvents.AddItem(i);
            }
       
            Assert.IsNotNull(overflowMessage);
            Assert.AreEqual("Переполнение очереди!", overflowMessage);
            
        }

        [Test]
        public void AnalyzeStreamWithConstantTest()
        {
            var numberStream = new NumberStream(new Collection<double>{2.0, 12.99, 321, 421, 42144443.91});
            string differenceEventMessage = null;
            numberStream.TooBigDifferenceEvent += delegate(string message)
            {
                differenceEventMessage = message;
            };
            numberStream.AnalyzeStreamWithConstant(1);
            Assert.IsNotNull(differenceEventMessage);
            Console.WriteLine(differenceEventMessage);
        }
        
        [Test]
        public void AnalyzeStreamWithCPercentageTest()
        {
            var numberStream = new NumberStream(new Collection<double>{2.0, 12.99, 321, 421, 42144443.91});
            string differenceEventMessage = null;
            numberStream.TooBigDifferenceEvent += delegate(string message)
            {
                differenceEventMessage = message;
            };
            numberStream.AnalyzeStreamWithPercentage(15);
            Assert.IsNotNull(differenceEventMessage);
            Console.WriteLine(differenceEventMessage);
        }
    }
}