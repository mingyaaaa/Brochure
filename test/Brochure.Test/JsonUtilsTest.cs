using AutoFixture;
using Brochure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Test
{
    [TestClass]
    public class JsonUtilsTest
    {
        /// <summary>
        /// Tests the merge configuration.
        /// </summary>
        [TestMethod]
        public void TestMergeConfiguration()
        {
            var builder = new ConfigurationBuilder();
            var configuration1 = builder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                ["a"] = "a",
                ["b"] = "b",
            }).Build();

            var configuration2 = builder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                ["a"] = "aa",
                ["c"] = "c",
            }).Build();

            JsonUtil ins = new JsonUtil();
            var r = ins.MergeConfiguration(configuration1, configuration2);
            Assert.AreEqual(r.GetValue<string>("a"), "a");
            Assert.AreEqual(r.GetValue<string>("b"), "b");
            Assert.AreEqual(r.GetValue<string>("c"), "c");

            r = ins.MergeConfiguration(configuration2, configuration1);
            Assert.AreEqual(r.GetValue<string>("a"), "aaa");
            Assert.AreEqual(r.GetValue<string>("b"), "b");
            Assert.AreEqual(r.GetValue<string>("c"), "c");
        }
    }
}
