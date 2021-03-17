using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using TwoLayerSolution.Extensions;

namespace Tests
{
    public class ExtensionTest
    {
        private enum Time
        {
            Hours,
            Minutes,
            Seconds,
            Milliseconds
        }

        private readonly Regex _secondsMinutesHoursRegex = new Regex("[0-9]?.[0-9]{2}:[0-9]{2}:[0-9]{2}");
        private readonly Regex _millisecondsMRegex = new Regex("[0-9]{2}:[0-9]{2}:[0-9]{2}.[0-9]?");

        private void ConvertToHoursWhichThrows()
        {
            var random = new Random();
            var testingValue = random.Next(600000000, 1000000000);
            testingValue.Hours();
        }
        
        private TimeSpan GenerateTime(Time time)
        {
            var random = new Random();
            var testingValue = random.Next(10000, 1000000);
            switch (time)
            {
                case Time.Hours:
                    return testingValue.Hours();
                case Time.Minutes:
                    return testingValue.Minutes();
                case Time.Seconds:
                    return testingValue.Seconds();
                case Time.Milliseconds:
                    return testingValue.Milliseconds();
            }
            return 0.Milliseconds();
        }

        private bool RegexTimeValidator(string toCheck, Time time)
        {
            return time == Time.Milliseconds ? _millisecondsMRegex.IsMatch(toCheck) : _secondsMinutesHoursRegex.IsMatch(toCheck);
        }

        [Test]
        public void IntToTimeSpanMillisecondsTest()
        {
            Console.WriteLine(GenerateTime(Time.Milliseconds).ToString());
            Assert.IsTrue(RegexTimeValidator(GenerateTime(Time.Milliseconds).ToString(), Time.Milliseconds));
        }
        
        [Test]
        public void IntToTimeSpanSecondsTest()
        {
            Assert.IsTrue(RegexTimeValidator(GenerateTime(Time.Seconds).ToString(), Time.Seconds));
        }
        
        [Test]
        public void IntToTimeSpanMinutesTest()
        {
            Assert.IsTrue(RegexTimeValidator(GenerateTime(Time.Minutes).ToString(), Time.Minutes));
        }
        
        [Test]
        public void IntToTimeSpanMHoursNormalTest()
        {
            Assert.IsTrue(RegexTimeValidator(GenerateTime(Time.Hours).ToString(), Time.Hours));
        }
        
        [Test]
        public void IntToTimeSpanMHoursOverflowTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(ConvertToHoursWhichThrows);
        }
    }
}