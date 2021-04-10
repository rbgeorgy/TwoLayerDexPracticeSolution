using System;
using System.Threading;

namespace TwoLayerSolution.ClassesUsingThreads
{
    public class ActionWrapper
    {
        public ActionWrapper(Action action)
        {
            Task = action;
        }
        public Action Task { get; private set;}
        
        public event EventHandler Finished; 
        //TODO: написать OnTaskFinished в TaskScheduler типа EventHandler 

        public void Invoke()
        {
            var lTaskThread = new Thread(() =>
            {
                Task.Invoke();
                var lFinished = Finished;
                lFinished?.Invoke(this, EventArgs.Empty);
            });
            
            lTaskThread.Start();
        }
    }
}