using System;
using System.Linq;
using Brochure.Core.Extends;
using Brochure.Core.Implements;
using Brochure.Core.Interfaces;
using Xunit;

namespace Brochure.Core.Test
{
    public class NewClassTest
    {
        #region BDDocment
        [Fact]
        public void NewBDDocment ()
        {
            var obj = new
            {
                ProInt = 1,
                ProTime = DateTime.Now,
                ProString = "ProString"
            };
            var doc = new Implements.Record (obj);
            Assert.Equal (obj.ProInt, doc[nameof (obj.ProInt)]);
            Assert.Equal (obj.ProTime, doc[nameof (obj.ProTime)]);
            Assert.Equal (obj.ProString, doc[nameof (obj.ProString)]);
            Assert.Equal (3, doc.Count);
            Assert.Equal (3, doc.Values.Count ());
            Assert.Equal (3, doc.Keys.Count ());
            Assert.NotNull (doc.As<string> ());
            foreach (var item in doc)
            {

            }
        }
        #endregion
    }
}