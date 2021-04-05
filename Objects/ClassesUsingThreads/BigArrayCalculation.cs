using System;
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
        public int start;
        public int end;
        public SmallBig whichArray;

        public ParametersForThread(int start, int end, SmallBig whichArray)
        {
            this.start = start;
            this.end = end;
            this.whichArray = whichArray;
        }
    }

    public class BigArrayCalculation
    {
        private int[] _smallArray;
        private int[] _bigArray;
        public List<double> AveragesForSmall;
        public List<double> AveragesForBig;

        public BigArrayCalculation()
        {
            AveragesForSmall = new List<double>();
            AveragesForBig = new List<double>();
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
            var threads = new Thread[whichArray == SmallBig.Big ? 100 : 10];
            for (var i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(GeneratePartial));
                threads[i].Start(i);
            }
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
            int cnt = 0;

            ParametersForThread parameters = (ParametersForThread) obj;
            var start = parameters.start;
            var end = parameters.end;
            var whichArray = parameters.whichArray;
            
            for(int i = start; i < end; i++)
            {
                try
                {
                    cnt++;
                    avgTemp += whichArray == SmallBig.Small ? _smallArray[i] : _bigArray[i];
                }
                catch (OverflowException e)
                {
                    if (whichArray == SmallBig.Small) 
                        AveragesForSmall.Add(avgTemp / cnt);
                    else 
                        AveragesForBig.Add(avgTemp/cnt);
                    cnt = 0;
                    avgTemp = 0;
                    Console.WriteLine(e);
                }
            }
        }

        public void CalculateAverageWithThreads(SmallBig whichArray)
        {
            var threads = new Thread[whichArray == SmallBig.Big ? 100 : 10];
            var parameters = new ParametersForThread[threads.Length];
            const int step = 1000000;
            for (var i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(CalculateAverage));
                parameters[i] = new ParametersForThread(step*i, step*(i+1), whichArray);
                threads[i].Start(parameters[i]);
            }
        }

        public double CalculateAverageInSmall()
        {
            CalculateAverage(new ParametersForThread(0, _smallArray.Length, SmallBig.Small));
            return AveragesForSmall.Sum(avg => avg / AveragesForSmall.Count);
        }
        
        public double CalculateAverageInBig()
        {
            CalculateAverage(new ParametersForThread(0, _bigArray.Length, SmallBig.Big));
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
            return AveragesForBig.Sum(avg => avg / AveragesForBig.Count);
        }
    }
}