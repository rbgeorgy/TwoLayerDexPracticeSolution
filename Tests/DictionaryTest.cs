using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class DictionaryTest
    {
        private delegate bool ToMeasure<in T>(T person);

        private long MeasureExecutionTime<T>(ToMeasure<T> method, T person)
        {
            method.Invoke(person);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            method.Invoke(person);
            stopWatch.Stop();
            return stopWatch.ElapsedTicks;
        }

        [Test]
        public void DictionaryVsListSpeed100ElementsTest()
        {
            var generator = new PersonGenerator();
            var dictionary = generator.GeneratePersonWorkPlaceDictionary(100);
            var findInDictionaryTime =  MeasureExecutionTime(dictionary.ContainsKey, generator.GeneratePerson());
            var list = generator.GeneratePersonArray(100).ToList();
            var findInListTime = MeasureExecutionTime(list.Contains, generator.GeneratePerson());
            Assert.IsTrue(findInListTime <= findInDictionaryTime);
        }
        
        [Test]
        public void DictionaryVsListSpeedTest()
        {
            var generator = new PersonGenerator();
            var dictionary = generator.GeneratePersonWorkPlaceDictionary(100000);
            var findInDictionaryTime =  MeasureExecutionTime(dictionary.ContainsKey, generator.GeneratePerson());
            var list = generator.GeneratePersonArray(100000).ToList();
            var findInListTime = MeasureExecutionTime(list.Contains, generator.GeneratePerson());
            Assert.IsTrue(findInListTime > findInDictionaryTime);
        }

        [Test]
        public void DictionaryWithKeyWithBadHashCodeVsKeyWithNormalHashCode()
        {
            var generator = new PersonGenerator();
            var dictionary = generator.GeneratePersonWorkPlaceDictionary(10000);
            var dictionaryWithKeyWithBadHashCode = generator.GeneratePersonWithBadHashCodeWorkPlaceDictionary(10000);
            var findInDictionaryTime =  MeasureExecutionTime(dictionary.ContainsKey, generator.GeneratePerson());
            var findInDictionaryWithKeyWithBadHashCodeTime =  MeasureExecutionTime(dictionaryWithKeyWithBadHashCode.ContainsKey, generator.GeneratePersonWithBadHashCode());
            Assert.IsTrue(findInDictionaryWithKeyWithBadHashCodeTime > findInDictionaryTime);
        }

    }
}