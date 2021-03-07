using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TwoLayerSolution
{
    public class QueueWithEvents<T> where T : new()
    {
        private Collection<T> _elements; 
        private int _front = 0; 
        private int _rear = -1; 
        private int _max;

        public delegate void QueueExceptionNotifier(string toNotify);

        public event QueueExceptionNotifier QueueOverflow;
        public event QueueExceptionNotifier QueueUnderflow;
  
        public QueueWithEvents(int size)
        {
            _max = size;
            _elements = new Collection<T>();
            for (int i = 0; i < size; i++)
            {
                _elements.Add(new T());
            }
        }

        public void AddItem(T item)
        {
            if (_rear == _max - 1)
            {
                QueueOverflow?.Invoke("Переполнение очереди!");
                return;
            } 
            else { 
                _elements[++_rear] = item; 
            } 
        }

        public T GetItem()
        {
            if (_front == _rear + 1) { 
                QueueUnderflow?.Invoke("Очередь пуста!");
                throw new InvalidOperationException("Очередь пуста!");
            } 
            else {
                T toGet = _elements[_front++];
                return toGet; 
            }
        }

    }
}