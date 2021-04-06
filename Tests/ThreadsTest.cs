using System;
using System.Diagnostics;
using NUnit.Framework;
using TwoLayerSolution.ClassesUsingThreads;

namespace Tests
{
    public class ThreadsTest
    {
        public delegate void Generate();
        public delegate void GenerateWithThreads(SmallBig whichArray);

        public delegate double Calculate();

        public long GetGenerateTime(Generate function)
        {
            var timer = new Stopwatch();
            function.Invoke();
            
            timer.Start();
            function.Invoke();
            timer.Stop();
            return timer.ElapsedMilliseconds;
        }
        
        public long GetGenerateTime(GenerateWithThreads function, SmallBig whichArray)
        {
            var timer = new Stopwatch();
            function.Invoke(whichArray);
            
            timer.Start();
            function.Invoke(whichArray);
            timer.Stop();
            return timer.ElapsedMilliseconds;
        }

        public long GetGenerateTime(Calculate calculate, Generate clear)
        {
            calculate.Invoke();
            clear.Invoke();
            var timer = new Stopwatch();
            timer.Start();
            calculate.Invoke();
            timer.Stop();
            return timer.ElapsedMilliseconds;
        }

        [Test]
        public void GenerateWithNoThreadsVsWithThreadsSpeedTest()
        {
            var bigArrayCalculation = new BigArrayCalculation();
            var timeWithNoThreads = GetGenerateTime(bigArrayCalculation.Generate);
            var timeWithThreads = GetGenerateTime(bigArrayCalculation.GenerateUsingThreads, SmallBig.Small)
                                  + GetGenerateTime(bigArrayCalculation.GenerateUsingThreads, SmallBig.Big);
            
            Console.WriteLine("Миллисекунд без потоков: " + timeWithNoThreads);
            Console.WriteLine("Миллисекунд с потоками: " + timeWithThreads);
            Assert.IsTrue(timeWithNoThreads > timeWithThreads);
        }

        [Test]
        public void CalculateAverageWithoutThreadsVsWithThreadsSpeedTest()
        {
            var bigArrayCalculation = new BigArrayCalculation();
            bigArrayCalculation.Generate();
            
            var timeWithNoThreads =
                GetGenerateTime(bigArrayCalculation.CalculateAverageInSmall, bigArrayCalculation.Generate)
                + GetGenerateTime(bigArrayCalculation.CalculateAverageInBig, bigArrayCalculation.Generate);
            
            var timeWithThreads = GetGenerateTime(bigArrayCalculation.CalculateAverageInSmallWithThreads,
                                      bigArrayCalculation.Generate)
                                  + GetGenerateTime(bigArrayCalculation.CalculateAverageInBigWithThreads,
                                      bigArrayCalculation.Generate);
            
            Console.WriteLine("Миллисекунд без потоков: " + timeWithNoThreads);
            Console.WriteLine("Миллисекунд с потоками: " + timeWithThreads);
        }

        [Test]
        public void DoesAverageWorksGoodTest()
        {
            var bigArrayCalculation = new BigArrayCalculation();
            bigArrayCalculation.Generate();
            Console.WriteLine(bigArrayCalculation.CalculateAverageInBig());
            Console.WriteLine(bigArrayCalculation.CalculateAverageInBigWithThreads());
        }
    }
}