using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TwoLayerSolution.Exceptions;

namespace TwoLayerSolution.ClassesUsingThreads
{
    public class TaskScheduler : IJobExecutor
    {
        private Queue<Action> _queue;
        private object _queueLocker;
        
        private Dictionary<Guid, Action> _runningTasks;
        private object _runningTasksLocker;
        
        private bool _isMoving;

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
            _isMoving = true;
            
            var freeSpace = GetFreeSpace(maxConcurrent);
            SendMaximumPossibleCountOfTasksToRun(freeSpace);

            while (_isMoving)
            {
                freeSpace = WaitSomeMillisecondsAndGetFreeSpaceAfter(1, maxConcurrent);
                if (freeSpace != 0) SendMaximumPossibleCountOfTasksToRun(freeSpace);
            }

            var isComplete = false;

            while (!isComplete)
            {
                Thread.Sleep(20);
                
                lock(_runningTasksLocker)
                {
                    if (_runningTasks.Count == 0) isComplete = true;
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
            _isMoving = false;
        }

        public void Add(Action action)
        {
            if (action == null) throw new NullArgumentException(nameof(action), nameof(Add));
            lock (_queueLocker)
            {
                _queue.Enqueue(action);
            }
            Amount++;
        }

        public void Clear()
        {
            lock (_queueLocker)
            {
                _queue.Clear();   
            }
        }

        public int GetCountOfRunningThreads()
        {
            ThreadPool.GetMaxThreads(out var max, out _);
            ThreadPool.GetAvailableThreads(out var available, out _);
            return max - available;
        }

        private void MoveNextActionToRunningTasksFromQueue()
        {
            if (Amount == 0)
            {
                _isMoving = false;
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

        private int WaitSomeMillisecondsAndGetFreeSpaceAfter(int millisecondsCount, int maxConcurrent)
        {
            Thread.Sleep(millisecondsCount);
            return GetFreeSpace(maxConcurrent);
        }

        private void SendMaximumPossibleCountOfTasksToRun(int freeSpace)
        {
            if (Amount == 0) _isMoving = false;
            RunTasks(Amount <= freeSpace ? Amount : freeSpace);
        }
    }
}