using System;
using System.Collections.Generic;
using System.Linq;
using Brochure.Abstract;
using Brochure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Brochure.Core.Test
{
    public enum TestEnum
    {
        a = 1,
        b = 2
    }

    public class A
    {
        public string AStr { get; set; }
        public B BObject { get; set; }
        public int C { get; set; }
        public DateTime DateTime { get; set; }
    }
    public class B
    {
        public string BStr { get; set; }
        public int C { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class C : IBConverables<B>
    {
        public B Conver ()
        {
            return new B ()
            {
                BStr = "C"
            };
        }
    }

    [TestClass]
    public class AsTest
    {
        [TestMethod]
        public void StringTo ()
        {
            var str = "1";
            Assert.AreEqual (1, str.As<int> ());
            str = "aaa";
            try
            {
                str.As<int> ();

                Assert.IsTrue (false);
            }
            catch (Exception)
            {
                Assert.IsTrue (true);
            }
            str = "1.34";
            Assert.AreEqual (0, str.As<int> ());
            Assert.AreEqual (1.34, str.As<double> ());
            var date = "1992.3.6 13:6:7";
            date = "1992.3.6";
            Assert.AreEqual (1992, date.As<DateTime> ().Year);
            date = "1992.3.6 25:6:7";
            try
            {
                date.As<DateTime> ();
                Assert.IsTrue (false);
            }
            catch (Exception)
            {
                Assert.IsTrue (true);
            }
        }

        [TestMethod]
        public void EnumTo ()
        {
            //测试枚举
            var te = TestEnum.a;
            Assert.AreEqual (1, te.As<int> ());
            var i = 1;
            Assert.AreEqual (TestEnum.a, i.As<TestEnum> ());
            i = 3;
            try
            {
                Assert.AreEqual (TestEnum.a, i.As<TestEnum> ());
                Assert.IsTrue (false);
            }
            catch (Exception)
            {
                Assert.IsTrue (true);
            }
        }

        [TestMethod]
        public void IntTo ()
        {
            var i = 1;
            Assert.AreEqual (1, i.As<int> ());
            Assert.AreEqual (1, i.As<double> ());
            Assert.AreEqual ("1", i.As<string> ());
        }

        [TestMethod]
        public void DoubleTo ()
        {
            var d = 1.3;
            Assert.AreEqual (1, (int) d);
            //double 无法直接转int
            try
            {
                d.As<int> ();
                Assert.IsTrue (false);
            }
            catch (Exception)
            {
                Assert.IsTrue (true);
            }

            Assert.AreEqual (1.3, d.As<double> ());
            Assert.AreEqual ("1.3", d.As<string> ());
        }

        [TestMethod]
        public void DateTimeTo ()
        {
            //Given
            var date = DateTime.Now;
            date.As<string> ();
            try
            {
                date.As<int> ();
                Assert.IsTrue (false);
            }
            catch (Exception)
            {
                Assert.IsTrue (true);
            }
            //When

            //Then
        }

        [TestMethod]
        public void NUllTo ()
        {
            object obj = null;
            Assert.AreEqual (0, obj.As<int> ());
            Assert.AreEqual (0, obj.As<double> ());
            Assert.IsNull (obj.As<string> ());
            Assert.AreEqual (default (DateTime), obj.As<DateTime> ());

        }

        [TestMethod]
        public void RecordTo ()
        {
            var a = new A ();
            a.AStr = "AStr";
            a.C = 0;
            a.DateTime = DateTime.Now;
            var b = new B ();
            b.BStr = "BStr";
            b.C = 1;
            b.DateTime = DateTime.Now.AddDays (-1);
            a.BObject = b;
            var ar = a.As<IRecord> ();
            Assert.IsTrue (ar.ContainsKey (nameof (a.AStr)));
            Assert.IsTrue (ar.ContainsKey (nameof (a.BObject)));
            Assert.IsTrue (ar.ContainsKey (nameof (a.C)));
            Assert.IsTrue (ar.ContainsKey (nameof (a.DateTime)));
            Assert.AreEqual (a.AStr, ar[nameof (a.AStr)]);
            Assert.AreEqual (a.C, ar[nameof (a.C)]);
            Assert.AreEqual (a.DateTime, ar[nameof (a.DateTime)]);
            Assert.AreEqual (a.AStr, ar[nameof (a.AStr)]);
            var br = ar[nameof (a.BObject)].As<IRecord> ();
            Assert.AreEqual (b.BStr, br[nameof (b.BStr)]);
            Assert.AreEqual (b.C, br[nameof (b.C)]);
            Assert.AreEqual (b.DateTime, br[nameof (b.DateTime)]);
        }

        [TestMethod]
        public void JsonTo ()
        {
            string json = "{\"aa\":1}";
            var obj = json.AsObject<IRecord> ();
            Assert.IsTrue (obj.ContainsKey ("aa"));
            Assert.AreEqual (1, obj["aa"].As<int> ());
            obj = json.As<IRecord> ();
            Assert.IsTrue (obj.ContainsKey ("aa"));
            Assert.AreEqual (1, obj["aa"].As<int> ());

            var jsonArray = new string[]
            {
                "a",
                "b",
                "c"
            };
            json = JsonConvert.SerializeObject (jsonArray);
            var array = json.AsEnumerable<string> ().ToArray ();
            Assert.AreEqual (3, array.Length);

            var recordArray = new List<IRecord>
            {
                new Record ()
                {
                ["a"] = 1,
                },
                new Record ()
                {
                ["b"] = 2,
                },
                new Record ()
                {
                ["c"] = 3,
                },
            };

            json = JsonConvert.SerializeObject (recordArray);
            var records = json.AsEnumerable<IRecord> ().ToArray ();
            Assert.AreEqual (3, records.Length);
            Assert.IsTrue (records[0].ContainsKey ("a"));
            Assert.AreEqual (1, records[0]["a"].As<int> ());
            records = json.AsEnumerable<IRecord> ().ToArray ();
            Assert.AreEqual (3, records.Length);
            Assert.IsTrue (records[0].ContainsKey ("a"));
            Assert.AreEqual (1, records[0]["a"].As<int> ());
        }

        [TestMethod]
        public void IBConverTo ()
        {
            var c = new C ();
            var b = c.As<B> ();
            Assert.AreEqual ("C", b.BStr);
        }
    }
}