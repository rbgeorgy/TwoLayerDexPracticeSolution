using System;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class ExtensionTest
    {
        private int _testingValue;
        private Random _random;

        [SetUp]
        public void SetUp()
        {
            _random = new Random();
            _testingValue = _random.Next();
        }

        [Test]
        public void IntToTimeSpanMillisecondsTest()
        {
            Console.WriteLine(_testingValue.Milliseconds());
        }
        
        [Test]
        public void IntToTimeSpanSecondsTest()
        {
            Console.WriteLine(_testingValue.Seconds());
        }
        
        [Test]
        public void IntToTimeSpanMinutesTest()
        {
            Console.WriteLine(_testingValue.Minutes());
        }
        
        [Test]
        public void IntToTimeSpanMHoursOverflowTest()
        {
            _testingValue = _random.Next(600000000, 1000000000);
            try
            {
                Console.WriteLine(_testingValue.Hours());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        [Test]
        public void IntToTimeSpanMHoursNormalTest()
        {
            _testingValue = _random.Next(-1000, 1000);
            Console.WriteLine(_testingValue.Hours());
        }
    }
}