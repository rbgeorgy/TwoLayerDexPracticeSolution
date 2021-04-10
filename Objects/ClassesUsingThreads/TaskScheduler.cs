using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace TwoLayerSolution.ClassesUsingThreads
{
    public class TaskScheduler : IJobExecutor
    {
        private Queue<Action> _queue;
        private object _queueLocker;
        
        private List<Action> _runningTasks;
        private object _runningTasksLocker;

        private int _maxConcurrent;
        private bool _isMooving;

        public TaskScheduler()
        {
            _queue = new Queue<Action>();
            _runningTasks = new List<Action>();
        }

        public int Amount { get; private set; }

        
        
        public void Start(int maxConcurrent)
        {
            _maxConcurrent = maxConcurrent;
        }

        public void Stop()
        {
            _isMooving = false;
        }

        public void Add(Action action)
        {
            //TODO: Exceptions
            if (action == null) throw new ArgumentNullException(nameof(action));
            ThreadPool.QueueUserWorkItem(state=>{action();});
        }

        public void Clear()
        {
            lock (_queueLocker)
            {
                _queue.Clear();
            }
        }

        private void OnTaskEnded(object sender, EventArgs e)
        {
            if (sender is Action actionWrapperToRemove)
            {
                lock (_runningTasksLocker)
                {
                    _runningTasks.Remove(actionWrapperToRemove);
                }
            }
            else throw new ArgumentException("Это событие должно быть для Action");
        }
    }
}