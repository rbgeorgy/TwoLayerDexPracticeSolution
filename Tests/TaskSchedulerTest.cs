using System;
using System.Threading;
using NUnit.Framework;

namespace Tests
{
    public class TaskSchedulerTest
    {
        public void SlowOperationTwoSecondsOperationWithId()
        {
            var id = Guid.NewGuid();
            Thread.Sleep(700);
            Console.WriteLine(id);
        }

        [Test]
        public void TaskSchedulerSimpleTest()
        {
            var scheduler = new TwoLayerSolution.ClassesUsingThreads.TaskScheduler();
            var threadsCount = 5;
            
            Action action = SlowOperationTwoSecondsOperationWithId;

            for (var i = 0; i < threadsCount * 2; i++)
            {
                scheduler.Add(action);
            }

            scheduler.Start(threadsCount);

            ThreadPool.GetMaxThreads(out var max, out _);
            ThreadPool.GetAvailableThreads(out var available, out _);
            var running = max - available;

            Console.WriteLine("Число работающих тредов: " + running);
            Assert.True(running - 1 == threadsCount);
        }
        
    }
}