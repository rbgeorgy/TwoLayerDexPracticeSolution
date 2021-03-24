using System;

namespace TwoLayerSolution
{
    [Serializable]
    public class SerializableProductWithOwner : Product, IEquatable<SerializableProductWithOwner>
    {
        public Person Owner { get; set; }

        public SerializableProductWithOwner()
        {
            Owner = new Person();
        }

        public SerializableProductWithOwner(string name, int number, DateTime time, bool isAvailable) : base(name, number, time, isAvailable)
        {
        }

        public SerializableProductWithOwner(Product product, Person person) : base(product.ProductName, product.Number, product.Time,
            product.IsAvailable)
        {
            Owner = person;
        }

        public bool Equals(SerializableProductWithOwner other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(Owner, other.Owner);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SerializableProductWithOwner) obj);
        }
    }
}