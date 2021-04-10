using System;
using System.Collections.Concurrent;
using System.Threading;

namespace TwoLayerSolution.ClassesUsingThreads
{
    public class TaskScheduler : IJobExecutor
    {
        private ConcurrentQueue<Action> _queue;
        private ConcurrentDictionary<WaitHandle, Action> _runningTasks;

        private int _maxConcurrent;
        private bool _isMooving;

        public TaskScheduler()
        {
            _queue = new ConcurrentQueue<Action>();
            _runningTasks = new ConcurrentDictionary<WaitHandle, Action>();
        }

        public int Amount { get; private set; }

        
        
        public void Start(int maxConcurrent)
        {
            _maxConcurrent = maxConcurrent;
            _isMooving = true;
            var vacantPlaces = _maxConcurrent - _runningTasks.Count;
            if (Amount > vacantPlaces)
            {
                for (int i = 0; i < vacantPlaces; i++)
                {
                    
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
            if (action == null) throw new ArgumentNullException(nameof(action));
            _queue.Enqueue(action);
            Amount++;
        }

        public void Clear()
        {
            if (_queue.IsEmpty) return;
            _queue = new ConcurrentQueue<Action>(); 
        }
    }
}