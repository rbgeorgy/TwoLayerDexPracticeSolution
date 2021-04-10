using System;
using System.Threading;
using NUnit.Framework;

namespace Tests
{
    public class TaskSchedulerTest
    {
        public void WriteRedText()
        {
            Console.WriteLine("Red Text");
        }
        
        public void WriteBlueText()
        {
            Console.WriteLine("Blue Text");
        }
        
        public void WriteWhiteText()
        {
            Console.WriteLine("White Text");
        }

        [Test]
        public void TaskSchedulerSimpleTest()
        {
            var scheduler = new TwoLayerSolution.ClassesUsingThreads.TaskScheduler();
            Action action = WriteRedText;
            scheduler.Add(action);
            
            action = WriteBlueText;
            scheduler.Add(action);
            
            action = WriteWhiteText;
            scheduler.Add(action);

            scheduler.Start(3);
            scheduler.Stop();
            
        }
        
    }
}