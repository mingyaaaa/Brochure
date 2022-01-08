using System;
using System.Linq;
using Brochure.Abstract.Extensions;
using Brochure.Abstract.Models;
using Brochure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Core.Test
{
    /// <summary>
    /// The new class test.
    /// </summary>
    [TestClass]
    public class NewClassTest
    {
        #region BDDocment

        /// <summary>
        /// News the b d docment.
        /// </summary>
        [TestMethod]
        public void NewBDDocment()
        {
            var obj = new
            {
                ProInt = 1,
                ProTime = DateTime.Now,
                ProString = "ProString"
            };
            var doc = new Record(obj.AsDictionary());
            Assert.AreEqual(obj.ProInt, doc[nameof(obj.ProInt)]);
            Assert.AreEqual(obj.ProTime, doc[nameof(obj.ProTime)]);
            Assert.AreEqual(obj.ProString, doc[nameof(obj.ProString)]);
            Assert.AreEqual(3, doc.Values.Count());
            Assert.AreEqual(3, doc.Keys.Count());
        }

        #endregion BDDocment
    }
}