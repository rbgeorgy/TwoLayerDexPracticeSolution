using System;
using System.Collections.ObjectModel;

namespace TwoLayerSolution
{
    public class UniqueCollection<T>
    {
        private Collection<T> _collection;

        public UniqueCollection()
        {
            _collection = new Collection<T>();
        }

        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException("Нельзя добавить null в колллекцию.");
            else
            {
                if (_collection.Contains(item))
                    throw new ArgumentException("Не удалось добавить "+ item +".\nКоллекция может содержать только уникальные экземпляры.");
                else _collection.Add(item);
            }
        }

        public void Print()
        {
            Console.WriteLine("Коллекция содержит элементы {0}:", typeof(T));
            foreach (T item in _collection)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }

    }
    
}