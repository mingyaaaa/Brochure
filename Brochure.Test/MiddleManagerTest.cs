using System;
using System.Linq;
using Brochure.Core.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    [TestClass]
    public class MiddleManagerTest
    {
        [TestMethod]
        public void TestAddSameMiddle ()
        {
            var middle = new MiddleManager ();
            middle.AddMiddle ("a", Guid.NewGuid (), t => t);
            Assert.ThrowsException<Exception> (() => middle.AddMiddle ("a", Guid.NewGuid (), t => t));
        }

        [TestMethod]
        public void TestAddIndexMiddle ()
        {
            var middle = new MiddleManager ();
            middle.AddMiddle ("a", Guid.NewGuid (), t => t);
            var list = middle.GetMiddlesList ();
            var first = list.FirstOrDefault ();
            Assert.IsNotNull (first);
            Assert.AreEqual (1, first.Order);
            middle.AddMiddle ("b", Guid.NewGuid (), t => t);
            first = list.FirstOrDefault (t => t.MiddleName == "b");
            Assert.IsNotNull (first);
            Assert.AreEqual (2, first.Order);
        }

        [TestMethod]
        public void TestIndexMiddle ()
        {
            var middle = new MiddleManager ();
            middle.AddMiddle ("a", Guid.NewGuid (), t => t);
            middle.IntertMiddle ("b", Guid.NewGuid (), 1, t => t);
            var list = middle.GetMiddlesList ();
            var first = list.FirstOrDefault (t => t.MiddleName == "b");
            Assert.AreEqual (1, first.Order);
            first = list.FirstOrDefault (t => t.MiddleName == "a");
            Assert.AreEqual (2, first.Order);
        }

        [TestMethod]
        public void TestIndexMaxIntOrder ()
        {
            var middle = new MiddleManager ();
            middle.AddMiddle ("a", Guid.NewGuid (), t => t);
            middle.IntertMiddle ("b", Guid.NewGuid (), int.MaxValue, t => t);
            middle.IntertMiddle ("c", Guid.NewGuid (), int.MaxValue, t => t);
            var list = middle.GetMiddlesList ();
            var first = list.FirstOrDefault (t => t.MiddleName == "c");
            Assert.AreEqual (int.MaxValue, first.Order);
            first = list.FirstOrDefault (t => t.MiddleName == "b");
            Assert.AreEqual (int.MaxValue - 1, first.Order);

            middle.IntertMiddle ("d", Guid.NewGuid (), int.MaxValue - 1, t => t);
            list = middle.GetMiddlesList ();
            first = list.FirstOrDefault (t => t.MiddleName == "b");
            Assert.AreEqual (int.MaxValue - 2, first.Order);
        }

    }
}