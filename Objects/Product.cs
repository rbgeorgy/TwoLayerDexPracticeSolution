using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TwoLayerSolution
{
    [Serializable]
    public class Product : INotifyPropertyChanged, IEquatable<Product>
    {
        private String _productName;
        public readonly int Number;
        public readonly DateTime Time;
        public readonly bool IsAvailable;

        public Product()
        {
            _productName = "DefaultProductName";
            Number = 1;
            Time = DateTime.Today;
            IsAvailable = true;
        }

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

        public bool Equals(Product other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _productName == other._productName && Number == other.Number && Time.Equals(other.Time) && IsAvailable == other.IsAvailable;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Product) obj);
        }
    }
}