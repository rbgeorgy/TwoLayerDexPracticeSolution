using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class SerializationTest
    {
        [Test]
        public void BinarySerializationTest()
        {
            var generator = new PersonGenerator();
            var person = generator.GeneratePerson();
            var product = new Product("Apple", 1, DateTime.Today, true);
            var serializableProduct = new SerializableProduct(product, person);
            BinaryFormatter formatter = new BinaryFormatter();

            using (var fileStream = new FileStream("SerializableProduct.bat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fileStream, serializableProduct);
            }
        }
    }
}