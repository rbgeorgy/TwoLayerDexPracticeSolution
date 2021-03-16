using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TwoLayerSolution;
using TwoLayerSolution.Enums;

namespace Tests
{
    public class QueryableInterfaceTest
    {
        private readonly string[] _productNames = { "Samsung", "Apple", "Huawei", "Nokia",
            "Sony", "HTC", "Motorola", "Lenovo",
            "Xiaomi", "Lg", "Meizu", "Asus", "ZTE", "Acer" };

        private enum MoreThenLessThen { MoreThan, LessThan }

        private DateTime UnixTimeStampToDateTime( double unixTimeStamp )
        {
            DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
            return dtDateTime;
        }

        private bool NextBoolean(Random random)
        {
            return random.Next() > (Int32.MaxValue / 2);
        }
        
        private Product[] Generate()
        {
            var rand = new Random();
            var products = new Product[100];
            for (int i = 0; i < 100; ++i)
            {
                int rndIndex = rand.Next(_productNames.Length);
                int rndNumber = rand.Next(0, 100);
                int rndUnixTime = rand.Next(1610000000,1614082083);
                products[i] = new Product(_productNames[rndIndex], rndNumber, UnixTimeStampToDateTime(rndUnixTime), NextBoolean(rand));
                rand.Next(0, 100);
            }
            return products;
        }

        private void Print(IEnumerable<Product> collection, ProductEnum whatToPrint)
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

        private bool CheckAllTime(Product[] products, int threshold, MoreThenLessThen parameter)
        {
            foreach (var item in products)
            {
                if ( parameter == MoreThenLessThen.MoreThan ?
                    ((DateTimeOffset) item.Time).ToUnixTimeSeconds() < threshold :
                    ((DateTimeOffset) item.Time).ToUnixTimeSeconds() > threshold
                    ) return false;
            }

            return true;
        }
        
        private bool CheckAnyTime(Product[] products, int threshold, MoreThenLessThen parameter)
        {
            foreach (var item in products)
            {
                if ( parameter == MoreThenLessThen.MoreThan ?
                    ((DateTimeOffset) item.Time).ToUnixTimeSeconds() > threshold :
                    ((DateTimeOffset) item.Time).ToUnixTimeSeconds() < threshold
                    ) return true;
            }
            return false;
        }

        [Test]
        public void WhereOrderByQueryTest()
        {
            var products = Generate();
            var numberQueryResult = products.Where(p => p.Number % 2 == 0).OrderBy(p => p.Number);
            Print(numberQueryResult, ProductEnum.Number);
        }

        [Test]
        public void GroupBySelectQueryTest()
        {
            var products = Generate();
            var nameQueryResult = products.GroupBy(p => p.ProductName.Length).Select(g => new {Length = g.Key, Words = g});
            foreach (var item in nameQueryResult)
            {
                Console.WriteLine("Brands with length " + item.Words.Key + ":");
                Print(item.Words, ProductEnum.Name);
            }
        }

        [Test]
        public void AllAnyQueryTest()
        {
            var products = Generate();
            var timeQueryAllResult = products.All(p => ((DateTimeOffset) p.Time).ToUnixTimeSeconds() > 1614000000);
            Assert.IsTrue(timeQueryAllResult
                ? CheckAllTime(products, 1614000000, MoreThenLessThen.MoreThan)
                : CheckAnyTime(products, 1614000000, MoreThenLessThen.LessThan));
            var timeQueryAnyResult = products.Any(p => ((DateTimeOffset) p.Time).ToUnixTimeSeconds() > 1614000000);
            Assert.IsTrue(timeQueryAnyResult
                ? CheckAnyTime(products, 1614000000, MoreThenLessThen.MoreThan)
                : CheckAllTime(products, 1614000000, MoreThenLessThen.LessThan));
        }

        [Test]
        public void WhereGroupBySelectTest()//TODO: ASSERTION
        {
            var products = Generate();
            var isAvailableQueryResult = products.Where(p => p.IsAvailable).GroupBy(p => p.ProductName)
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