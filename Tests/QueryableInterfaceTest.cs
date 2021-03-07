using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class QueryableInterfaceTest
    {
        private Product[] _products;

        private readonly string[] _productNames = { "Samsung", "Apple", "Huawei", "Nokia",
            "Sony", "HTC", "Motorola", "Lenovo",
            "Xiaomi", "Lg", "Meizu", "Asus", "ZTE", "Acer" };

        private DateTime _unixTimeStampToDateTime( double unixTimeStamp )
        {
            DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
            return dtDateTime;
        }

        private bool _nextBoolean(Random random)
        {
            return random.Next() > (Int32.MaxValue / 2);
        }
        
        private void _generate()
        {
            var rand = new Random();
            _products = new Product[100];
            for (int i = 0; i < 100; ++i)
            {
                int rndIndex = rand.Next(_productNames.Length);
                int rndNumber = rand.Next(0, 100);
                int rndUnixTime = rand.Next(1610000000,1614082083);
                _products[i] = new Product(_productNames[rndIndex], rndNumber, _unixTimeStampToDateTime(rndUnixTime), _nextBoolean(rand));
                rand.Next(0, 100);
            }
        }

        private void _print(IEnumerable<Product> collection, ProductEnum whatToPrint)
        {
            foreach (var item in collection)
            {
                switch (whatToPrint)
                {
                    case ProductEnum.Name:
                        Console.WriteLine(item.ProductName);
                        break;
                    case ProductEnum.Number:
                        Console.WriteLine(item.Number);
                        break;
                    case ProductEnum.Time:
                        Console.WriteLine(item.Time);
                        break;
                    default: Console.WriteLine(item.IsAvailable);
                        break;
                }
            }
        }
        
        [SetUp]
        public void SetUp()
        {
            _generate();
        }
        
        [Test]
        public void WhereAndOrderByQueryTest()
        {
            var numberQueryResult = _products.Where(p => p.Number % 2 == 0).OrderBy(p => p.Number);
            _print(numberQueryResult, ProductEnum.Number);
        }

        [Test]
        public void GroupByAndSelectQueryTest()
        {
            var nameQueryResult = _products.GroupBy(p => p.ProductName.Length).Select(g => new {Length = g.Key, Words = g});
            foreach (var item in nameQueryResult)
            {
                Console.WriteLine("Brands with length " + item.Words.Key + ":");
                _print(item.Words, ProductEnum.Name);
            }
        }

        [Test]
        public void AllAndAnyQueryTest()
        {
            String timeQueryResult = _products.All(p => ((DateTimeOffset) p.Time).ToUnixTimeSeconds() > 1614000000)
                ? "Все даты позднее, чем " + _unixTimeStampToDateTime(1614000000)
                : _products.Any(p => ((DateTimeOffset) p.Time).ToUnixTimeSeconds() > 1614000000)
                    ? "Есть даты не позднее, чем " + _unixTimeStampToDateTime(1614000000)
                    : "Нет ни одной даты позднее, чем " + _unixTimeStampToDateTime(1614000000);
            Console.WriteLine(timeQueryResult);
        }

        [Test]
        public void WhereAndGroupByAndSelectTest()
        {
            var isAvailableQueryResult = _products.Where(p => p.IsAvailable).GroupBy(p => p.ProductName)
                .Select(g => new {Name = g.Key, productsWithCurrentName = g});
            List<int> sums = new List<int>();
            foreach (var item in isAvailableQueryResult)
            {
                int checkSum = item.productsWithCurrentName.Sum(p => p.Number);
                sums.Add(checkSum);
            }
            
            Console.WriteLine("Максимальное количество доступного товара одного бренда: " + sums.Max());
            Console.WriteLine("Минимальное количество доступного товара одного бренда: " + sums.Min());
        }
    }
}