using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace TwoLayerSolution.ClassesUsingThreads
{
    public class TaskScheduler : IJobExecutor
    {
        private Queue<Action> _queue;
        //private object _queueLocker;
        
        private Dictionary<Guid, Action> _runningTasks;
        private object _runningTasksLocker;

        private int _maxConcurrent;
        private bool _isMooving;

        public TaskScheduler()
        {
            //_queueLocker = new object();
            _runningTasksLocker = new object();
            _queue = new Queue<Action>();
            _runningTasks = new Dictionary<Guid, Action>();
        }

        public int Amount { get; private set; }

        public void Start(int maxConcurrent)
        {
            _maxConcurrent = maxConcurrent;
            _isMooving = true;

            while (_isMooving)
            {
                int freeSpace;
                lock (_runningTasksLocker)
                {
                    freeSpace = maxConcurrent - _runningTasks.Count;
                }
                if (Amount <= freeSpace)
                {
                    for (var i = 0; i < Amount;)
                    {
                        MoveNextActionToRunningTasksFromQueue();
                    }
                }
                else
                    for (var i = 0; i < freeSpace; freeSpace--)
                        {
                            MoveNextActionToRunningTasksFromQueue();
                        }
            }
        }

        public void Stop()
        {
            _isMooving = false;
        }

        public void Add(Action action)
        {
            //TODO: Exceptions
            if (action == null) throw new ArgumentNullException("Action не может быть null!");
            
            //lock (_queueLocker) {
                _queue.Enqueue(action);
                Amount++;
            //}
        }

        public void Clear()
        {
            //lock (_queueLocker)
            //{
                _queue.Clear();
            //}
        }

        private void MoveNextActionToRunningTasksFromQueue()
        {
            if (Amount == 0)
            {
                _isMooving = false;
                return;
            }

            ThreadPool.QueueUserWorkItem(state =>
            {
                var id = Guid.NewGuid();
                Action action;
                // lock (_queueLocker)
                // { 
                    action = _queue.Dequeue();
                //}

                lock (_runningTasksLocker)
                {
                    _runningTasks[id] = action;
                    Amount--;
                }
                
                try
                {
                    action();
                }
                
                finally
                {
                    lock (_runningTasksLocker)
                    {
                        _runningTasks.Remove(id);
                    }
                }
            });
        }
    }
}