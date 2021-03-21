using System;

namespace TwoLayerSolution
{
    [Serializable]
    public class SerializableProduct : Product
    {

        public Person Owner { get; set; }

        public SerializableProduct(string name, int number, DateTime time, bool isAvailable) : base(name, number, time, isAvailable)
        {
        }

        public SerializableProduct(Product product, Person person) : base(product.ProductName, product.Number, product.Time,
            product.IsAvailable)
        {
            Owner = person;
        }
    }
}