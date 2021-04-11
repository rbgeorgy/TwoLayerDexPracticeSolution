using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using TwoLayerSolution.Exceptions;

namespace TwoLayerSolution.ClassesUsingThreads
{
    public class TaskScheduler : IJobExecutor
    {
        private Queue<Action> _queue;
        private object _queueLocker;
        
        private Dictionary<Guid, Action> _runningTasks;
        private object _runningTasksLocker;
        
        private bool _isMooving;
        private bool _areTasksRunning;

        public TaskScheduler()
        {
            _runningTasksLocker = new object();
            _queueLocker = new object();
            
            _queue = new Queue<Action>();
            _runningTasks = new Dictionary<Guid, Action>();
        }

        public int Amount { get; private set; }

        public void Start(int maxConcurrent)
        {
            _isMooving = true;
            _areTasksRunning = true;

            while (_areTasksRunning)
            {
                Thread.Sleep(1);
                var freeSpace = GetFreeSpace(maxConcurrent);
                RunTasks(freeSpace);
                lock (_runningTasksLocker)
                {
                    if (_runningTasks.Count == 0 && _isMooving == false) 
                        _areTasksRunning = false;
                }
            }
            
        }

        private int GetFreeSpace(int maxConcurrent)
        {
            int freeSpace;
            
            lock (_runningTasksLocker)
            {
                freeSpace = maxConcurrent - _runningTasks.Count;
            }
            
            return freeSpace;
        }

        public void Stop()
        {
            _isMooving = false;
        }

        public void Add(Action action)
        {
            if (action == null) throw new NullArgumentException(nameof(action), nameof(Add));
            _queue.Enqueue(action);
            Amount++;
        }

        public void Clear()
        {
            lock (_queueLocker)
            {
                _queue.Clear();   
            }
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
                
                lock (_queueLocker)
                {
                    action = _queue.Dequeue();   
                }

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

        private void RunTasks(int count)
        {
            for (var i = 0; i < count; i++)
            {
                MoveNextActionToRunningTasksFromQueue();
            }
        }
    }
}