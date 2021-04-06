using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TwoLayerSolution.ClassesUsingThreads
{
    public enum SmallBig
    {
        Small, Big
    }
    public class ParametersForThread
    {
        public readonly int Index;
        public readonly int Start;
        public readonly int End;
        public readonly SmallBig WhichArray;

        public ParametersForThread(int index, int start, int end, SmallBig whichArray)
        {
            Index = index;
            Start = start;
            End = end;
            WhichArray = whichArray;
        }
    }

    public class BigArrayCalculation
    {
        private int[] _smallArray;
        private int[] _bigArray;
        private WaitHandle[] _waitHandles;
        public ConcurrentBag<double> AveragesForSmall;
        public ConcurrentBag<double> AveragesForBig;

        public BigArrayCalculation()
        {
            AveragesForSmall = new ConcurrentBag<double>();
            AveragesForBig = new ConcurrentBag<double>();
        }

        public void Clear()
        {
            AveragesForSmall = new ConcurrentBag<double>();
            AveragesForBig = new ConcurrentBag<double>();
        }

        public void Generate()
        {
            var rand = new Random();
            _smallArray = new int[10000000];
           
            for (var i = 0; i < _smallArray.Length; i++)
            {
                _smallArray[i] = rand.Next(0, 1000000);
            }

            _bigArray = new int[100000000];
            for (var i = 0; i < _bigArray.Length; i++)
            {
                _bigArray[i] = rand.Next(0, 1000000);
            }
        }

        public void GenerateUsingThreads(SmallBig whichArray)
        {
            var rand = new Random();
            _smallArray = new int[10000000];
            var threads = new Thread[whichArray == SmallBig.Big ? 64 : 10];
            for (var i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(GeneratePartial));
                threads[i].Start(i);
            }
        }

        public void GenerateUsingThreads()
        {
            GenerateUsingThreads(SmallBig.Small);
            GenerateUsingThreads(SmallBig.Big);
        }

        private void GeneratePartial(object obj)
        {
            var rand = new Random();
            var step = (int) obj;
            
            for (var i = 1000000*step; i < 1000000*(step + 1); i++)
            {
                _smallArray[i] = rand.Next(0, 1000000);
            }
        }

        private void CalculateAverage(object obj)
        {
            double avgTemp = 0;
            var parameters = (ParametersForThread) obj;

            var index = parameters.Index;
            var start = parameters.Start;
            var end = parameters.End;
            var whichArray = parameters.WhichArray;
            var handle = new EventWaitHandle(false, EventResetMode.ManualReset);
            handle.Set();
            _waitHandles[index] = handle;
            
            for(int i = start; i < end; i++)
            {
                avgTemp += (whichArray == SmallBig.Small) ? _smallArray[i] : _bigArray[i];
                
                if (i % 2000 == 0 && i !=0)
                {
                    if (whichArray == SmallBig.Small)
                    {
                        AveragesForSmall.Add(avgTemp / 2000);
                    }
                    else
                    {
                        AveragesForBig.Add(avgTemp/2000);
                    }
                    avgTemp = 0;
                }
            }
        }

        public void CalculateAverageWithThreads(SmallBig whichArray)
        {
            var length = whichArray == SmallBig.Big ? 64 : 10;
            var threads = new Thread[length];
            _waitHandles = new WaitHandle[length];
            var parameters = new ParametersForThread[threads.Length];
            const int step = 1000000;
            
            for (var i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(CalculateAverage));
                parameters[i] = new ParametersForThread(i, step*i, step*(i+1), whichArray);
                threads[i].Start(parameters[i]);
            }
            WaitHandle.WaitAll(_waitHandles);
        }

        public double CalculateAverageInSmall()
        {
            CalculateAverage(new ParametersForThread(0, 0, _smallArray.Length, SmallBig.Small));
            return AveragesForSmall.Sum(avg => avg / AveragesForSmall.Count);
        }
        
        public double CalculateAverageInBig()
        {
            CalculateAverage(new ParametersForThread(0,0, _bigArray.Length, SmallBig.Big));
            return AveragesForBig.Sum(avg => avg / AveragesForBig.Count);
        }
        
        public double CalculateAverageInSmallWithThreads()
        {
            CalculateAverageWithThreads(SmallBig.Small);
            return AveragesForSmall.Sum(avg => avg / AveragesForSmall.Count);
        }
        
        public double CalculateAverageInBigWithThreads()
        {
            CalculateAverageWithThreads(SmallBig.Big);
            //Thread.Sleep(400);
            return AveragesForBig.Sum(avg => avg / AveragesForBig.Count);
        }
    }
}