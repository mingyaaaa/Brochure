using System;
using System.Linq;
using Brochure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Brochure.Core.Test
{
    [TestClass]
    public class NewClassTest
    {
        #region BDDocment
        [TestMethod]
        public void NewBDDocment ()
        {
            var obj = new
            {
                ProInt = 1,
                ProTime = DateTime.Now,
                ProString = "ProString"
            };
            var doc = new Record (obj.AsDictionary ());
            Assert.AreEqual (obj.ProInt, doc[nameof (obj.ProInt)]);
            Assert.AreEqual (obj.ProTime, doc[nameof (obj.ProTime)]);
            Assert.AreEqual (obj.ProString, doc[nameof (obj.ProString)]);
            Assert.AreEqual (3, doc.Values.Count ());
            Assert.AreEqual (3, doc.Keys.Count ());
            Assert.IsNotNull (doc.As<string> ());
            Assert.AreEqual (JsonConvert.SerializeObject (obj.AsDictionary ()), doc.As<string> ());
            foreach (var item in doc)
            {

            }
        }
        #endregion
    }
}