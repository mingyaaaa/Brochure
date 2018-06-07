using Xunit;
using Brochure.Core.Implements;
using Brochure.Core.Interfaces;
using Brochure.Core.Extends;
using System;
using System.Linq;

namespace Brochure.Core.Test
{
    public class NewClassTest
    {
        #region BDDocment
        [Fact]
        public void NewBDDocment()
        {
            var obj = new
            {
                ProInt = 1,
                ProTime = DateTime.Now,
                ProString = "ProString"
            };
            var doc = new BDocument(obj);
            Assert.Equal(obj.ProInt, doc[nameof(obj.ProInt)]);
            Assert.Equal(obj.ProTime, doc[nameof(obj.ProTime)]);
            Assert.Equal(obj.ProString, doc[nameof(obj.ProString)]);
            Assert.Equal(3, doc.Count);
            Assert.Equal(3, doc.Values.Count());
            Assert.Equal(3, doc.Keys.Count());
            Assert.NotNull(doc.As<string>());
            foreach (var item in doc)
            {

            }
        }
        #endregion
    }
}