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
        //TODO: Переработать для параллельных запусков; Переопределить ==; HashCode, Equals
        //TODO: исследовать скорость работы словаря, в случае если хеш-код всегда один и тот же
        private delegate bool ToMeasure(Person person);

        private long MeasureExecutionTime(ToMeasure method, Person person)
        {
            method.Invoke(person);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            method.Invoke(person);
            stopWatch.Stop();
            return stopWatch.ElapsedTicks;
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

    }
}