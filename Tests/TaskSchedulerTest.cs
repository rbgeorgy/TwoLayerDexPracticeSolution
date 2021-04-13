using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;

namespace Tests
{
    public class TaskSchedulerTest
    {
        public delegate void TaskForScheduler();
        public TwoLayerSolution.ClassesUsingThreads.TaskScheduler GetScheduler(int itemsCount, TaskForScheduler task)
        {
            var scheduler = new TwoLayerSolution.ClassesUsingThreads.TaskScheduler();

            Action action = task.Invoke;

            for (var i = 0; i < itemsCount; i++)
            {
                scheduler.Add(action);
            }

            return scheduler;
        }

        public void TwoSecondsSlowOperationWithId()
        {
            var id = Guid.NewGuid();
            Thread.Sleep(1000);
            Console.WriteLine(id);
        }

        public void FiveSecondsSlowOperation()
        {
            Thread.Sleep(5000);
            Console.WriteLine("OK");
        }

        [Test]
        public void AreAllThreadsCompletedTest()
        {
            var threadsCount = 5;
            var scheduler = GetScheduler(15, TwoSecondsSlowOperationWithId);
            scheduler.Start(threadsCount);
            Assert.AreEqual(2, scheduler.GetCountOfRunningThreads());
        }

        [Test]
        public void AreTasksReallyWorkParallelTest()
        {
            var threadsCount = 3;
            var scheduler = GetScheduler(3, FiveSecondsSlowOperation);
            var time = new Stopwatch();
            time.Start();
            scheduler.Start(threadsCount);
            time.Stop();
            Assert.IsTrue(time.ElapsedMilliseconds <= 5100);
        }

        [Test]
        public void TaskSchedulerClearWhileDequeuingTest()
        {
            var threadsCount = 17;
            var scheduler = GetScheduler(14, TwoSecondsSlowOperationWithId);
            scheduler.Start(threadsCount);
            scheduler.Clear();
        }

    }
}