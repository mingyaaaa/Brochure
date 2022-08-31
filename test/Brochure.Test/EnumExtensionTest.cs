using Brochure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Test
{
    [TestClass]
    public class EnumExtensionTest
    {
        [TestMethod]
        public void TestGetDes()
        {
            var des = TestEnum.A.GetDescript();
            Assert.AreEqual("A", des);
            des = TestEnum.B.GetDescript();
            Assert.AreEqual("B", des);
        }

        public enum TestEnum
        {
            [System.ComponentModel.Description("A")]
            A,

            [System.ComponentModel.Description("B")]
            B,
        }
    }
}