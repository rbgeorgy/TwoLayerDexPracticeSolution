using System;

namespace TwoLayerSolution.ClassesUsingThreads
{
    public class BigArrayCalculation
    {
        private int[] _smallArray;
        private int[] _bigArray;

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

        public double GetAverage(int[] array)
        {
            double avg = 0;
            double avgTemp = 0;
            int cnt = 0;
            
            foreach (var item in array)
            {
                try
                {
                    cnt++;
                    avgTemp += item;
                }
                catch (OverflowException e)
                {
                    avg += avgTemp/cnt;
                    cnt = 0;
                    avgTemp = 0;
                    Console.WriteLine(e);
                }
            }
            return avg;
        }

        public double GetAverageInSmall()
        {
            return GetAverage(_smallArray);
        }
        
        public double GetAverageInBig()
        {
            return GetAverage(_bigArray);
        }
    }
}