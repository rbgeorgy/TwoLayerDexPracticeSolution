using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TwoLayerSolution
{
    public class Product : INotifyPropertyChanged
    {
        private String _productName;
        public readonly int Number;
        public readonly DateTime Time;
        public readonly bool IsAvailable;

        public Product(String name, int number, DateTime time, bool isAvailable)
        {
            ProductName = name ?? throw new ArgumentException("Имя не может быть null!");
            Number = number;
            Time = time;
            IsAvailable = isAvailable;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string ProductName
        {
            get => this._productName;
            set
            {
                if (value == this._productName) return;
                this._productName = value;
                NotifyPropertyChanged();
            }
        }

        public override string ToString()
        {
            return _productName;
        }
    }
}