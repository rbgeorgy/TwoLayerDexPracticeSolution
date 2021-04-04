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

        public void CalculateAverage(object obj)
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
            for (int i = 0; i < 5; i++)
            {
                
            }

            var firstThread = new Thread(new ParameterizedThreadStart(CalculateAverage));
            var secondThread = new Thread(new ParameterizedThreadStart(CalculateAverage));
            var thirdThread = new Thread(new ParameterizedThreadStart(CalculateAverage));
            var fourthThread = new Thread(new ParameterizedThreadStart(CalculateAverage));
            var fifthThread = new Thread(new ParameterizedThreadStart(CalculateAverage));

            var firstParameters = new ParametersForThread(0, 2000000, whichArray);
            var secondParameters = new ParametersForThread(2000000, 4000000, whichArray);
            var thirdParameters = new ParametersForThread(4000000, 6000000, whichArray);
            var fourthParameters = new ParametersForThread(6000000, 8000000, whichArray);
            var fifthParameters = new ParametersForThread(8000000, 10000000, whichArray);
            
            firstThread.Start(firstParameters);
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
    }
}