using System;
using System.Collections.Generic;
using Brochure.Core.Extends;
using Brochure.Core.Interfaces;
using System.Linq;
using System.Reflection;

namespace Brochure.Core.Abstracts
{
    public abstract class AbSingleton
    {
        private static object lockObj = new object();
        protected AbSingleton()
        {
            var type = this.GetType();
            if (type == typeof(AbSingleton))
                throw new TypeLoadException("无法创建AbSingleton的实例");
            if (_dic.ContainsKey(type))
                throw new TypeLoadException("单例类无法创建对象,请通过GetInstace方法获取实例对象");
            lock (lockObj)
            {
                if (!_dic.ContainsKey(type))
                    _dic.Add(type, this);
            }
        }
        public static IDictionary<Type, AbSingleton> _dic = new Dictionary<Type, AbSingleton>();
        public static T GetInstance<T>()
        {
            var type = typeof(T);
            var insType = type;
            //如果获取抽象类 或接口的实现类
            if (type.IsAbstract)
                insType = type.GetSonTypes().FirstOrDefault();
            if (type.IsInterface)
                insType = type.GetSonInterfaceType().FirstOrDefault();
            if (insType == null)
                throw new TypeLoadException("当前抽象类没有继承没有被实现，无法初始化");
            var inter = insType.GetBaseType("AbSingleton");
            if (inter == null)
                throw new TypeLoadException("类型没有继承AbSingleton类");
            if (!_dic.ContainsKey(insType))
                insType.Assembly.CreateInstance(insType.FullName);
            return (T)(object)_dic[insType];
        }
    }
}