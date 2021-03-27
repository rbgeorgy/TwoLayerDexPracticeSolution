using System;
using System.Collections.Generic;

namespace TwoLayerSolution.FigureInterfaces
{
    public interface IFigureCollectionReverser<out T>
    {
        public IEnumerable<T> ReverseAndThrowEvent();
    }
}