using Brochure.Abstract;
using Brochure.Abstract.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    [TestClass]
    public class GenFunTest
    {
        //   [TestMethod]
        public void MyTestMethod()
        {
            var fun = new GenFun();
            fun.Get();
        }
    }

    public class GenFun
    {
        public T Get<T>() where T : new()
        {
            return new T();
        }

        public T Get<T>(object obj) where T : class
        {
            return obj as T;
        }

        public IRecord Get()
        {
            return Get<IRecord>(new Record());
        }
    }
}