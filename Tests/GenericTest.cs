using System;
using System.Collections.ObjectModel;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class GenericTest
    {
        private Person _personOne;
        private Person _personTwo;
        private Person _personThree;
        private delegate void AddFunction<T>(UniqueCollection<T> uniqueCollection, Collection<T> toAdd);
        
        private void TryAddingAnElementToUniqueCollection<T>(AddFunction<T> addFunction, Collection<T> toAdd)
        {
            UniqueCollection<T> uniqueCollection = new UniqueCollection<T>();
            try
            {
                addFunction(uniqueCollection, toAdd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                uniqueCollection.Print();
            }
        }
        
        private void AddFunctionMethod<T>(UniqueCollection<T> uniqueCollection, Collection<T> toAdd)
        {
            foreach (T item in toAdd)
            {
                uniqueCollection.Add(item);
            }
        }

        [SetUp]
        public void SetUp()
        {
            _personOne = new Person(
                "Ivanov Ivan Ivanovich",
                "01.01.1990",
                "Nezavertailovka",
                "AA2281337"
            );
            
            _personTwo = new Person(
                "Alekseyev Artyom Ivanovich",
                "21.01.1991",
                "Moscow",
                "AA1488146"
            );

            _personThree = new Person(
                "Ivanov Ivan Ivanovich",
                "01.01.1990",
                "Nezavertailovka",
                "AA2281337"
            );
        }

        [Test]
        public void UniqueCollectionAddIntExpectingExceptionTest()
        {
            TryAddingAnElementToUniqueCollection(AddFunctionMethod<int>, new Collection<int>{1, 2, 3, 1});
        }
        
        [Test]
        public void UniqueCollectionAddDoubleTest()
        {
            TryAddingAnElementToUniqueCollection(AddFunctionMethod<double>, new Collection<double>{2.1545, 4312, Math.PI, 1});
        }
        
        [Test]
        public void UniqueCollectionAddPersonExpectingExceptionTest()
        {
            TryAddingAnElementToUniqueCollection(AddFunctionMethod<Person>, new Collection<Person> {_personOne, _personTwo, _personThree});
        }

    }
}