using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class GenericTest
    {
        private delegate void AddFunction<T>(UniqueCollection<T> uniqueCollection, ICollection<T> toAdd);
        
        private void TryAddingAnElementToUniqueCollection<T>(AddFunction<T> addFunction, ICollection<T> toAdd)
        {
            UniqueCollection<T> uniqueCollection = new UniqueCollection<T>();

                addFunction(uniqueCollection, toAdd);
        }
        
        private void AddFunctionMethod<T>(UniqueCollection<T> uniqueCollection, ICollection<T> toAdd)
        {
            foreach (T item in toAdd)
            {
                uniqueCollection.Add(item);
            }
        }

        private void ThrowArgumentExceptionWhileAddExistingInt()
        {
            TryAddingAnElementToUniqueCollection(AddFunctionMethod<int>, new Collection<int> {1, 2, 3, 1});
        }
        
        private void ThrowArgumentExceptionWhileAddExistingPerson()
        {
            var generator = new PersonGenerator();
            var list = (generator.GeneratePersonArray(3)).ToList();
            list.Add(list[0]);
            TryAddingAnElementToUniqueCollection<Person>(AddFunctionMethod<Person>, list);
        }
        
        private void DoesNotThrowArgumentExceptionWhileAddExistingDouble()
        {
            TryAddingAnElementToUniqueCollection(AddFunctionMethod<double>, new Collection<double>{2.1545, 4312, Math.PI, 1});
        }

        [Test]
        public void UniqueCollectionAddIntExpectingExceptionTest()
        {
            Assert.Throws<ArgumentException>(ThrowArgumentExceptionWhileAddExistingInt);
        }
        
        [Test]
        public void UniqueCollectionAddDoubleTest()
        {
            Assert.DoesNotThrow(DoesNotThrowArgumentExceptionWhileAddExistingDouble);
        }
        
        [Test]
        public void UniqueCollectionAddPersonExpectingExceptionTest()
        {
            Assert.Throws<ArgumentException>(ThrowArgumentExceptionWhileAddExistingPerson);
        }

    }
}