using System;

namespace TwoLayerSolution
{
    public static class IntForTimeSpanExtension
    {
        public static TimeSpan Milliseconds(this int value)
        {
            return new TimeSpan(0, 0, 0, 0, Math.Abs(value));
        }
        public static TimeSpan Seconds(this int value)
        {
            return new TimeSpan(0, 0, Math.Abs(value));
        }
        
        public static TimeSpan Minutes(this int value)
        {
            return new TimeSpan(0, Math.Abs(value), 0);
        }
        
        public static TimeSpan Hours(this int value)
        {
            try
            {
                return new TimeSpan(Math.Abs(value), 0, 0);
            }
            catch (Exception e)
            {
                throw new ArgumentOutOfRangeException("Переполнение TimeSpan!");
            }
        }
    }
}