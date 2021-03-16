using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class DictionaryTest
    {
        //TODO: Переработать для параллельных запусков; Переопределить ==; HashCode, Equals
        //TODO:
        private EmployeeCatalog _employeeCatalog;

        [SetUp]
        public void SetUp()
        {
            _employeeCatalog = new EmployeeCatalog(new Dictionary<Person, string>
            {
            });
        }
    }
}