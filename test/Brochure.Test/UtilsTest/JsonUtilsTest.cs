using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test.UtilsTest
{
    [TestClass]
    public class JsonUtilsTest
    {
        //[TestMethod]
        //public void TestObjJsonValue()
        //{
        //    var json = "{\"aaa\":1,\"bbb\":\"aaa\"}";
        //    var jsonutil = new JsonUtil();
        //    Assert.IsTrue(jsonutil.ObjectJsonValid(json));
        //    Assert.IsFalse(jsonutil.ArrayJsonValid(json));
        //    Assert.IsTrue(jsonutil.IsJson(json));
        //}

        //[TestMethod]
        //public void TestArrayJsonValue()
        //{
        //    var json = "[\"aaa\",\"bbb\",\"aaa\"]";
        //    var jsonutil = new JsonUtil();
        //    Assert.IsFalse(jsonutil.ObjectJsonValid(json));
        //    Assert.IsTrue(jsonutil.ArrayJsonValid(json));
        //    Assert.IsTrue(jsonutil.IsJson(json));
        //}

        //[TestMethod]
        //[DataRow("{\"aaa\":1,\"bbb\":\"aaa\"}", true)]
        //[DataRow("[\"aaa\",\"bbb\",\"aaa\"]", true)]
        //[DataRow("[\"aaa\",\"bbb\",\"aaa\",{\"aaa\":1,\"bbb\":\"aaa\"}]", true)]
        //[DataRow("[\"aaa\",\"bbb\",\"aaa\",[\"aaa\",\"bbb\",\"aaa\"]]", true)]
        //[DataRow("[\"aaa\",\"bbb\",\"aaa\" [\"aaa\",\"bbb\",\"aaa\"]", false)]
        //[DataRow("[\"aaa\",\"bbb\",\"aaa\" ,{\"aaa\",\"bbb\",\"aaa\"}", false)]
        //public void TestIsJson(string str, bool r)
        //{
        //    var jsonutil = new JsonUtil();
        //    Assert.AreEqual(r, jsonutil.IsJson(str));
        //}
    }
}