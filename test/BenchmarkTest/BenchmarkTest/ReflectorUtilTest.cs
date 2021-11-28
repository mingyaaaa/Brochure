using BenchmarkDotNet.Attributes;
using BenchmarkTest.Models;
using Brochure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkTest.BenchmarkTest
{
    public class ReflectorUtilTest
    {
        public static int Count = 10000;

        [Benchmark]
        public void TestGetProperty()
        {
            var obj = new UserEntrity()
            {
                Age = 1,
                Name = "a",
            };
            var fun = ReflectorUtil.Instance.GetPropertyValueFun(obj.GetType(), "Age");
            for (int i = 0; i < Count; i++)
            {
                var value = fun.Invoke(obj);
            }
        }

        [Benchmark]
        public void TestGetProperty1()
        {
            var obj = new UserEntrity()
            {
                Age = 1,
                Name = "a",
            };
            var fun = ReflectorUtil.Instance.GetPropertyValueFun<UserEntrity>("Age");
            for (int i = 0; i < Count; i++)
            {
                var value = fun.Invoke(obj);
            }
        }

        [Benchmark]
        public void TestGetProperty2()
        {
            var obj = new UserEntrity()
            {
                Age = 1,
                Name = "a",
            };
            var type = obj.GetType();
            var property = type.GetProperty("Age");
            for (int i = 0; i < Count; i++)
            {
                var value = property.GetGetMethod().Invoke(obj, null);
            }
        }

        [Benchmark]
        public void TestGetProperty3()
        {
            var obj = new UserEntrity()
            {
                Age = 1,
                Name = "a",
            };
            var type = obj.GetType();
            var property = type.GetProperty("Age");
            for (int i = 0; i < Count; i++)
            {
                var value = property.GetValue(obj, null);
            }
        }
    }
}