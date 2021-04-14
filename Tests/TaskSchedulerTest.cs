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
            scheduler.Join();
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
            scheduler.Join();
            time.Stop();
            Console.WriteLine(time.ElapsedMilliseconds);
            Assert.IsTrue(time.ElapsedMilliseconds <= 6000);
        }

        [Test]
        public void ClearWhileDequeuingTest()
        {
            var threadsCount = 2;
            var scheduler = GetScheduler(14, TwoSecondsSlowOperationWithId);
            Assert.AreEqual(14,scheduler.Amount);
            scheduler.Start(threadsCount);
            Thread.Sleep(2500);
            
            Assert.DoesNotThrow(() =>
            {
                scheduler.Clear(); 
            });
            
            Assert.AreEqual(0, scheduler.Amount);
            scheduler.Join();
        }

        [Test]
        public void AddingWhileDequeuingAndRunningTasks()
        {
            var threadsCount = 2;
            var scheduler = GetScheduler(14, TwoSecondsSlowOperationWithId);
            scheduler.Start(threadsCount);
            Thread.Sleep(500);
            var amount = scheduler.Amount;
            Assert.DoesNotThrow(() =>
            {
                scheduler.Add(TwoSecondsSlowOperationWithId);
            });
            Assert.AreEqual(amount + 1, scheduler.Amount);
            scheduler.Join();
        }

    }
}