using System;
using System.Collections.Generic;
using System.Reflection;

namespace TwoLayerSolution
{
    public class MyDelegate
    {
        private List<MethodInfo> _listOfMethods;

        public List<MethodInfo> ListOfMethods => _listOfMethods;

        public ParametersAndType SignatureOfFunction;
        public MyDelegate(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            _listOfMethods = new List<MethodInfo> {method};
            SignatureOfFunction = new ParametersAndType(method.GetParameters(), method.ReturnType);
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
                SignatureOfFunction = new ParametersAndType(method.GetParameters(), method.ReturnType);
            }
            if (method.ReturnType != SignatureOfFunction.ReturnType || !ParametersInfoEquality(method.GetParameters(), SignatureOfFunction.Parameters)
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
                for (int i = 0; i < first.ListOfMethods.Count; i++)
                {
                    newList.Add(first.ListOfMethods[i]);
                    if (i == first.ListOfMethods.Count - 1) i = 0;
                }
                first._listOfMethods = newList;
            }

            foreach (var methodInfo in second.ListOfMethods)
            {
                first.AddMethod(methodInfo);
            }

            return first;
        }

        public static MyDelegate operator -(MyDelegate first, MyDelegate second)
        {
            if (!first.SignatureOfFunction.Equals(second.SignatureOfFunction))
            {
                throw new InvalidOperationException("Сигнатуры функций не совпадают");
            }
            
            if (first == second)
            {
                first._listOfMethods.Clear();
                first.SignatureOfFunction.Clear();
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
            if (_listOfMethods.Count == 0) return null;
            else
            {
                foreach (var methodInfo in _listOfMethods)
                {
                    try
                    {
                        return methodInfo.Invoke(classInstance, parameters);
                    }
                    catch (Exception exception)
                    {
                        if (exception.InnerException != null) Console.WriteLine(exception.InnerException.Message);
                    }
                }
            }
            return null;
        }
    }
}