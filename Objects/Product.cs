using System;

namespace TwoLayerSolution
{
    public class Product
    {
        public readonly String Name;
        public readonly int Number;
        public readonly DateTime Time;
        public readonly bool IsAvailable;

        public Product(String name, int number, DateTime time, bool isAvailable)
        {
            Name = name ?? throw new ArgumentException("Имя не может быть null!");
            Number = number;
            Time = time;
            IsAvailable = isAvailable;
        }

    }
}