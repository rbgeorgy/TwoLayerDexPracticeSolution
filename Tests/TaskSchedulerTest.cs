using System;
using System.Threading;
using NUnit.Framework;

namespace Tests
{
    public class TaskSchedulerTest
    {
        public TwoLayerSolution.ClassesUsingThreads.TaskScheduler GetScheduler(int itemsCount)
        {
            var scheduler = new TwoLayerSolution.ClassesUsingThreads.TaskScheduler();

            Action action = SlowOperationTwoSecondsOperationWithId;

            for (var i = 0; i < itemsCount; i++)
            {
                scheduler.Add(action);
            }

            return scheduler;
        }

        public void SlowOperationTwoSecondsOperationWithId()
        {
            var id = Guid.NewGuid();
            Thread.Sleep(1000);
            Console.WriteLine(id);
        }

        [Test]
        public void TaskSchedulerSimpleTest()
        {
            var threadsCount = 20;
            var scheduler = GetScheduler(45);

            scheduler.Start(threadsCount);

            ThreadPool.GetMaxThreads(out var max, out _);
            ThreadPool.GetAvailableThreads(out var available, out _);
            var running = max - available;

            Console.WriteLine("Число работающих тредов: " + running);
            //Assert.True(running - 1 == threadsCount);
        }

        [Test]
        public void TaskSchedulerClearWhileDequeuingTest()
        {
            var threadsCount = 255;
            var scheduler = GetScheduler(100);
            scheduler.Start(threadsCount);
            scheduler.Clear();
        }

    }
}