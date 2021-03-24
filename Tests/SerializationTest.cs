using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
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
            var serializableProduct = new SerializableProductWithOwner(product, person);
            BinaryFormatter formatter = new BinaryFormatter();

            using (var fileStream = new FileStream("SerializableProduct.bat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fileStream, serializableProduct);
            }

            using (var fileStream = new FileStream("SerializableProduct.bat", FileMode.OpenOrCreate))
            {
                var deserializedProduct = (SerializableProductWithOwner) formatter.Deserialize(fileStream);
                Assert.AreEqual(serializableProduct, deserializedProduct);
            }
        }

        [Test]
        public void XmlSerializationTest()
        {
            var xmlSerializer = new XmlSerializer(typeof(SerializableProductWithOwner));
            var serializableProduct = new SerializableProductWithOwner();
            using (var fileStream = new FileStream("SerializableProduct.xml", FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fileStream, serializableProduct);
            }

            using (var fileStream = new FileStream("SerializableProduct.xml", FileMode.OpenOrCreate))
            {
                var deserializedProduct = (SerializableProductWithOwner) xmlSerializer.Deserialize(fileStream);
                Assert.AreEqual(serializableProduct, deserializedProduct);
            }
        }
    }
}