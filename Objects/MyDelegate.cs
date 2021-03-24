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
        public MyDelegate()
        {
            _listOfMethods = new List<MethodInfo>();
        }

        public MyDelegate(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            _listOfMethods = new List<MethodInfo> {method};
            SignatureOfFunction = new ParametersAndType(method.GetParameters(), method.ReturnType);
        }

        private bool ParametersInfoEquality(ParameterInfo[] first, ParameterInfo[] second)
        {
            Console.WriteLine(first.Length + " " + second.Length);
            
            if (first.Length != second.Length) return false;
            for (int i = 0; i < first.Length; i++)
            {
                Console.WriteLine(first[i] + " " + second[i]);
                if (!first[i].Equals(second[i])) return false;
            }
            return true;
        }

        public void AddMethod(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (_listOfMethods.Count == 0)
            {
                SignatureOfFunction = new ParametersAndType(method.GetParameters(), method.ReturnType);
            }
            if (method.ReturnType != SignatureOfFunction.ReturnType
                //|| ParametersInfoEquality(method.GetParameters(), SignatureOfFunction.Parameters)
            )
            {
                throw new InvalidOperationException("Сигнатуры функций не совпадают");
            }

            _listOfMethods.Add(method);
        }
        public static MyDelegate operator +(MyDelegate first, MyDelegate second)
        {
            if (!first.SignatureOfFunction.Equals(second.SignatureOfFunction))
            {
                throw new InvalidOperationException("Сигнатуры функций не совпадают");
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return null;
        }
    }
}