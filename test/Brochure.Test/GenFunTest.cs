using Brochure.Abstract;
using Brochure.Abstract.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    /// <summary>
    /// The gen fun test.
    /// </summary>
    [TestClass]
    public class GenFunTest
    {
        //   [TestMethod]
        /// <summary>
        /// Mies the test method.
        /// </summary>
        public void MyTestMethod()
        {
            var fun = new GenFun();
            fun.Get();
        }
    }

    /// <summary>
    /// The gen fun.
    /// </summary>
    public class GenFun
    {
        /// <summary>
        /// Gets the.
        /// </summary>
        /// <returns>A T.</returns>
        public T Get<T>() where T : new()
        {
            return new T();
        }

        /// <summary>
        /// Gets the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A T.</returns>
        public T Get<T>(object obj) where T : class
        {
            return obj as T;
        }

        /// <summary>
        /// Gets the.
        /// </summary>
        /// <returns>An IRecord.</returns>
        public IRecord Get()
        {
            return Get<IRecord>(new Record());
        }
    }
}