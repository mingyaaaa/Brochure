using System;
using System.Linq;
using Brochure.Core.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    /// <summary>
    /// The middle manager test.
    /// </summary>
    [TestClass]
    public class MiddleManagerTest
    {
        /// <summary>
        /// Tests the add same middle.
        /// </summary>
        [TestMethod]
        public void TestAddSameMiddle()
        {
            var middle = new MiddleManager();
            middle.AddMiddle("a", Guid.NewGuid(), t => t);
            middle.AddMiddle("a", Guid.NewGuid(), t => t);
            var count = middle.GetMiddlesList().Count();
            Assert.AreEqual(1, count);
        }

        /// <summary>
        /// Tests the add index middle.
        /// </summary>
        [TestMethod]
        public void TestAddIndexMiddle()
        {
            var middle = new MiddleManager();
            middle.AddMiddle("a", Guid.NewGuid(), t => t);
            var list = middle.GetMiddlesList();
            var first = list.FirstOrDefault();
            Assert.IsNotNull(first);
            Assert.AreEqual(1, first.Order);
            middle.AddMiddle("b", Guid.NewGuid(), t => t);
            first = list.FirstOrDefault(t => t.MiddleName == "b");
            Assert.IsNotNull(first);
            Assert.AreEqual(2, first.Order);
        }

        /// <summary>
        /// Tests the index middle.
        /// </summary>
        [TestMethod]
        public void TestIndexMiddle()
        {
            var middle = new MiddleManager();
            middle.AddMiddle("a", Guid.NewGuid(), t => t);
            middle.IntertMiddle("b", Guid.NewGuid(), 1, t => t);
            var list = middle.GetMiddlesList();
            var first = list.FirstOrDefault(t => t.MiddleName == "b");
            Assert.AreEqual(1, first.Order);
            first = list.FirstOrDefault(t => t.MiddleName == "a");
            Assert.AreEqual(2, first.Order);
        }

        /// <summary>
        /// Tests the index max int order.
        /// </summary>
        [TestMethod]
        public void TestIndexMaxIntOrder()
        {
            var middle = new MiddleManager();
            middle.AddMiddle("a", Guid.NewGuid(), t => t);
            middle.IntertMiddle("b", Guid.NewGuid(), int.MaxValue, t => t);
            middle.IntertMiddle("c", Guid.NewGuid(), int.MaxValue, t => t);
            var list = middle.GetMiddlesList();
            var first = list.FirstOrDefault(t => t.MiddleName == "c");
            Assert.AreEqual(int.MaxValue, first.Order);
            first = list.FirstOrDefault(t => t.MiddleName == "b");
            Assert.AreEqual(int.MaxValue - 1, first.Order);

            middle.IntertMiddle("d", Guid.NewGuid(), int.MaxValue - 1, t => t);
            list = middle.GetMiddlesList();
            first = list.FirstOrDefault(t => t.MiddleName == "b");
            Assert.AreEqual(int.MaxValue - 2, first.Order);
        }
    }
}