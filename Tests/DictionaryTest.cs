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

        private delegate void ToMeasure2<in T>(T toAdd,
            Dictionary<PersonWithBadHashCode, string> dict, int length);

        private long MeasureExecutionTime<T>(ToMeasure<T> method, T person)
        {
            method.Invoke(person);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            method.Invoke(person);
            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedTicks);
            return stopWatch.ElapsedTicks;
        }

        private long Measure2ExecutionTime<T>(ToMeasure2<PersonWithBadHashCode> method, PersonWithBadHashCode person, Dictionary<PersonWithBadHashCode, string> dict, int length)
        {
            method.Invoke(person, dict, length);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            method.Invoke(person, dict, length);
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
        
        [Test]
        public void AddWithBadHashCodeVsNormalHashCode()
        {
            var generator = new PersonGenerator();
            var dictionaryWithKeyWithBadHashCode = generator.GeneratePersonWithBadHashCodeWorkPlaceDictionary(10000);
            var add10000ElementsWithBadHashCode =
                Measure2ExecutionTime<PersonWithBadHashCode>(generator.AddPersonWithBadHashCode, generator.GeneratePersonWithBadHashCode(), dictionaryWithKeyWithBadHashCode, 1000);
            Console.WriteLine(add10000ElementsWithBadHashCode);
        }
    }
}