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
        
        private void _tryAddingAnElementToUniqueCollection<T>(AddFunction<T> addFunction, Collection<T> toAdd)
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
        
        private void _addFunctionMethod<T>(UniqueCollection<T> uniqueCollection, Collection<T> toAdd)
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
            _tryAddingAnElementToUniqueCollection(_addFunctionMethod<int>, new Collection<int>{1, 2, 3, 1});
        }
        
        [Test]
        public void UniqueCollectionAddDoubleTest()
        {
            _tryAddingAnElementToUniqueCollection(_addFunctionMethod<double>, new Collection<double>{2.1545, 4312, Math.PI, 1});
        }
        
        [Test]
        public void UniqueCollectionAddPersonExpectingExceptionTest()
        {
            _tryAddingAnElementToUniqueCollection(_addFunctionMethod<Person>, new Collection<Person> {_personOne, _personTwo, _personThree});
        }

    }
}