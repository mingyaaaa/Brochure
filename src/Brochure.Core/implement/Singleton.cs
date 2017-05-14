using System;
using System.Collections.Generic;
using System.Text;
using Brochure.Core.Helper;

namespace Brochure.Core.implement
{
    public abstract class Singleton
    {
        private static readonly object obj = new object();
        private static object _instance;

        protected Singleton()
        {
            var type = GetType();
            if (_instance == null)
            {
                lock (obj)
                {
                    if (_instance != null)
                        throw new InvalidOperationException($"{type}为单例模式,无法创建多个实例");
                    _instance = new object();
                }
            }
        }
        public static T GetInstace<T>()
        {
            if (_instance == null)
            {
                lock (obj)
                {
                    if (_instance == null)
                        _instance = ObjectHelper.CreateInstance<T>();
                }
            }
            return (T)_instance;
        }
    }
}
