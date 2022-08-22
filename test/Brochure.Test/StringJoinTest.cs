using Brochure.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Test
{
    [TestClass]
    public class StringJoinTest
    {
        [TestMethod]
        public void TestJoin()
        {
            var join = new StringJoin("aa", ",");
            for (int i = 0; i < 3; i++)
            {
                join.Join("bb");
            }
            Assert.AreEqual("aa,bb,bb,bb", join.ToString());
        }
    }
}