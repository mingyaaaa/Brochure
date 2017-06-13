using System;
using System.Collections.Generic;
using System.Text;
using Brochure.Core.Helper;

namespace Brochure.Core.implement
{
    public abstract class Singleton
    {
        private static readonly object obj = new object();
        private static Dictionary<Type, Singleton> Dic = new Dictionary<Type, Singleton>();

        protected Singleton()
        {
            var type = GetType();
            if (!Dic.ContainsKey(type))
            {
                lock (obj)
                {
                    if (Dic.ContainsKey(type))
                        throw new InvalidOperationException($"{type}为单例模式,无法创建多个实例");
                    Dic[type] = this;
                }
            }
        }
        public static T GetInstace<T>() where T : Singleton, new()
        {
            var type = typeof(T);
            if (!Dic.ContainsKey(type))
            {
                lock (obj)
                {
                    if (!Dic.ContainsKey(type))
                        Dic[type] = new T();
                }
            }
            return (T)Dic[type];
        }
    }
}
