using System;
using Brochure.Core;
using Brochure.Core.Extends;
using Xunit;
using System.Threading.Tasks;
using test;
using Xunit.Abstractions;

namespace Tests
{
    public class Tests : BaseTest
    {
        public Tests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
        [Fact]
        public void Test1()
        {
            IDocument a = new RecordDocument(null);
            Assert.NotNull(a);
            Assert.Equal(0, a.Count);
            a = new RecordDocument(new { aa = 1, bb = 222 });
            Assert.NotNull(a);
            Assert.Equal(1, a["aa"]);
            Assert.Equal(222, a["bb"]);
            a = new RecordDocument(new { aa = 1, bb = new { cc = 33, dd = 44 } });
        }
        [Fact]
        public void Test3() => OutputHelper.WriteLine((16 | 4).ToString());

        [Fact]
        public void Test2Async()
        {
            TestClass test = new TestClass
            {
                Class1 = new Class1
                {
                    A = 1,
                    B = "aaaa"
                },
                C = "cccc"
            };
            var doc = test.AsDocument();
            var result = doc.ToEntrityObject<TestClass>();
            Assert.NotNull(result);
            Assert.NotNull(result.Class1);
            Assert.Equal("cccc", result.C);
        }
        [Fact]
        public async Task Test4Async()
        {
            await TestTask();
            OutputHelper.WriteLine("1");
        }


        public class TestClass : IEntrity
        {
            public Class1 Class1 { get; set; }

            public string C { get; set; }
            public string TableName { get; }
            public Guid Id { get; }
            public IModel ConverToDataModel()
            {
                throw new NotImplementedException();
            }
        }

        public class Class1
        {
            public int A { get; set; }
            public string B { get; set; }
        }

        public async Task TestTask()
        {
            OutputHelper.WriteLine("2");
            await Task.Delay(1000);
            OutputHelper.WriteLine("3");
        }
    }
}