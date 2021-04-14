using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TwoLayerSolution
{
    public class MyDelegate
    {
        private List<MethodInfo> _listOfMethods;

        public List<Exception> Exceptions { get; }

        public List<MethodInfo> ListOfMethods => _listOfMethods;

        private ParametersAndType _signatureOfFunction;
        public MyDelegate(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            _listOfMethods = new List<MethodInfo> {method};
            _signatureOfFunction = new ParametersAndType(method.GetParameters(), method.ReturnType);
            Exceptions = new List<Exception>();
        }

        private bool ParametersInfoEquality(ParameterInfo[] first, ParameterInfo[] second)
        {
            if (first.Length != second.Length) return false;
            for (int i = 0; i < first.Length; i++)
            {
                if (first[i].ParameterType != second[i].ParameterType) return false;
            }
            return true;
        }

        public void AddMethod(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (_listOfMethods.Count == 0)
            {
                _listOfMethods = new List<MethodInfo>();
                _signatureOfFunction = new ParametersAndType(method.GetParameters(), method.ReturnType);
            }
            if (method.ReturnType != _signatureOfFunction.ReturnType || !ParametersInfoEquality(method.GetParameters(), _signatureOfFunction.Parameters)
            )
            {
                throw new InvalidOperationException("Сигнатуры функций не совпадают");
            }

            _listOfMethods.Add(method);
        }
        public static MyDelegate operator +(MyDelegate first, MyDelegate second)
        {
            if (first == second)
            {
                var newList = new List<MethodInfo>();
                newList = newList.Concat(first.ListOfMethods).ToList();
                first._listOfMethods = newList.Concat(first.ListOfMethods).ToList();
                return first;
            }

            foreach (var methodInfo in second.ListOfMethods)
            {
                first.AddMethod(methodInfo);
            }

            return first;
        }

        public static MyDelegate operator -(MyDelegate first, MyDelegate second)
        {
            if (!first._signatureOfFunction.Equals(second._signatureOfFunction))
            {
                throw new InvalidOperationException("Сигнатуры функций не совпадают");
            }
            
            if (first == second)
            {
                first._listOfMethods.Clear();
                first._signatureOfFunction.Clear();
            }

            if (first.ListOfMethods.Count == 0) return first;
            
            foreach (var methodInfoInSecond in second.ListOfMethods)
            {
                foreach (var methodInfoInFirst in first.ListOfMethods)
                {
                    if (Equals(methodInfoInSecond, methodInfoInFirst)) first.ListOfMethods.Remove(methodInfoInFirst);
                }    
            }
            return first;
        }

        public object Invoke(object classInstance, object[] parameters)
        {
            Exceptions.Clear();
            object result = null;
            foreach (var methodInfo in _listOfMethods)
            {
                try
                {
                    result = methodInfo.Invoke(classInstance, parameters);
                }
                catch (Exception exception)
                {
                    if (exception.InnerException != null)
                        Exceptions.Add(exception.InnerException);
                }
            }

            Console.WriteLine("Делегат успешно выполнил работу. Число возникших исключений: " + Exceptions.Count);
            return result;
        }
    }
}