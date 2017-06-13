using System;
using System.Collections.Generic;
using System.Reflection;
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

        public static Singleton GetInstace(Type type)
        {
            if (!Dic.ContainsKey(type))
            {
                lock (obj)
                {
                    if (!Dic.ContainsKey(type))
                    {
#if net452

                        var con = type.GetConstructor(new Type[] { });
#else
                        var con = type.GetTypeInfo().GetConstructor(new Type[] { });
#endif

                        Dic[type] = (Singleton)con.Invoke(new object[] { });
                    }
                }
            }
            return Dic[type];
        }
    }
}
