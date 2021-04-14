using System;
using System.Collections.Generic;
using System.Threading;
using TwoLayerSolution.Exceptions;

namespace TwoLayerSolution.ClassesUsingThreads
{
    public class TaskScheduler : IJobExecutor
    {
        private Thread _startThread;
        private Thread _stopThread;
        private Thread _threadWhichMainThreadNeedToWait;

        private Queue<Action> _queue;
        private readonly object _queueLocker;
        
        private Dictionary<Guid, Action> _runningTasks;
        private readonly object _runningTasksLocker;
        
        private volatile bool  _queueIsntEmpty;
        private volatile bool _isQueueProcessingComplete;
        private volatile bool _isEverythingComplete;

        private const int StopDefaultTimeout = 3000;
        private const int RefreshTimeout = 5;
        
        private int _amount;
        public int Amount
        {
            get => Interlocked.CompareExchange(ref _amount, 0, 0);
            private set => Interlocked.Exchange(ref _amount, value);
        }
        
        public TaskScheduler()
        {
            _runningTasksLocker = new object();
            _queueLocker = new object();

            _queue = new Queue<Action>();
            _runningTasks = new Dictionary<Guid, Action>();
        }

        public void Start(int maxConcurrent)
        {
            if (maxConcurrent > 1020)
            {
                throw new NonValidValueException(nameof(maxConcurrent), nameof(Start), maxConcurrent);
            }

            _startThread = new Thread(() =>
            {
                _isQueueProcessingComplete = false;
                var freeSpace = GetFreeSpace(maxConcurrent);
                SendMaximumPossibleCountOfTasksToRun(freeSpace);

                while (!_isQueueProcessingComplete)
                {
                    freeSpace = WaitSomeMillisecondsAndGetFreeSpaceAfter(RefreshTimeout, maxConcurrent);
                    if (freeSpace != 0) SendMaximumPossibleCountOfTasksToRun(freeSpace);
                }

                if (!_queueIsntEmpty && AreAllTheTasksComplete())
                {
                    _isEverythingComplete = true;
                }
            });
            _startThread.Start();
            KillMainThreadIfEverythingComplete();
        }
        
        public void Stop()
        {
            _stopThread = new Thread(() =>
            {
                _isQueueProcessingComplete = true;

                while (!AreAllTheTasksComplete())
                {
                    Thread.Sleep(RefreshTimeout);
                }

                if (_queueIsntEmpty)
                {
                    Thread.Sleep(StopDefaultTimeout);
                    _isEverythingComplete = true;
                }
            });
            _stopThread.Start();
            KillMainThreadIfEverythingComplete();
        }

        public void Add(Action action)
        {
            if (action == null) throw new NullArgumentException(nameof(action), nameof(Add));
            lock (_queueLocker)
            {
                _queue.Enqueue(action);
            }
            _queueIsntEmpty = true;
            Interlocked.Increment(ref _amount);
        }

        public void Clear()
        {
            lock (_queueLocker)
            {
                _queue.Clear();
            }

            Amount = 0;
            _queueIsntEmpty = false;
        }
        
        public void Join()
        {
            _threadWhichMainThreadNeedToWait.Join();
        }

        private void KillMainThreadIfEverythingComplete()
        {
            _threadWhichMainThreadNeedToWait = new Thread(() =>
            {
                while (!_isEverythingComplete)
                {
                    if (!_queueIsntEmpty && AreAllTheTasksComplete())
                    {
                        _isEverythingComplete = true;
                    }
                    Thread.Sleep(RefreshTimeout);
                }
            });
            _threadWhichMainThreadNeedToWait.Start();
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

        private bool AreAllTheTasksComplete()
        {
            lock(_runningTasksLocker)
            {
                if (_runningTasks.Count == 0) return true;
            }
            return false;
        }
        
        private void MoveNextActionToRunningTasksFromQueue()
        {
            if (Amount == 0)
            {
                _queueIsntEmpty = false;
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
                    Interlocked.Decrement(ref _amount);
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
            if (Amount == 0)
            {
                _queueIsntEmpty = false;
            }
            RunTasks(Amount <= freeSpace ? Amount : freeSpace);
        }
        
        public int GetCountOfRunningThreads()
        {
            ThreadPool.GetMaxThreads(out var max, out _);
            ThreadPool.GetAvailableThreads(out var available, out _);
            return max - available;
        }
    }
}