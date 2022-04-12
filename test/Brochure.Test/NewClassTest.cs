using Brochure.Abstract.Extensions;
using Brochure.Abstract.Models;
using Brochure.Extensions;
using Mapster;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var doc = new Record(obj.Adapt<IDictionary<string, object>>());
            Assert.AreEqual(obj.ProInt, doc[nameof(obj.ProInt)]);
            Assert.AreEqual(obj.ProTime, doc[nameof(obj.ProTime)]);
            Assert.AreEqual(obj.ProString, doc[nameof(obj.ProString)]);
            Assert.AreEqual(3, doc.Values.Count());
            Assert.AreEqual(3, doc.Keys.Count());
        }

        #endregion BDDocment
    }
}