using System;
using System.Threading.Tasks;
using Brochure.Core;
using Brochure.Core.Abstract;
using Brochure.Core.Extends;
using Brochure.Core.implement;
using ConnectionDal;
using Xunit;
using Xunit.Abstractions;

namespace test
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

        [Fact]
        public void Test30()
        {
            var a = new Config();
            var b = new Setting();
            var aa = new Config();
        }

        public class TestClass : BaseEntrity
        {
            public Class1 Class1 { get; set; }

            public string C { get; set; }
            public override string TableName => "";

            public override IModel ConverToDataModel()
            {
                throw new NotImplementedException();
            }
        }

        public class Class1
        {
            public int A { get; set; }
            public string B { get; set; }
        }

        public class Config : Singleton
        {
        }

        private async Task TestTask()
        {
            OutputHelper.WriteLine("2");
            await Task.Delay(1000);
            OutputHelper.WriteLine("3");
        }
    }
}