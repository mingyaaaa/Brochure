using System;
using System.Linq;
using System.Reflection;

namespace Brochure.Core
{
    public class Mg
    {
        public static T Get<T>() where T : ISingleton
        {
#if net452
            var type = typeof(T).GetTypeInfo();
            Type ty = null;
            foreach (var item in type.Assembly.GetTypes())
            {

                var interfaces = item.GetInterfaces();
                var inter = interfaces.FirstOrDefault(t => t.FullName == type.FullName);
                if (inter != null)
                {
                    ty = item;
                    break;
                }
            }
            if (ty == null)
                throw new Exception("找不到该接口的实现类");
            var basetype = ty.BaseType;
            var method = basetype.GetMethod("GetInstace", new[] { typeof(Type) });
#else
            var type = typeof(T).GetTypeInfo();
            TypeInfo ty = null;
            foreach (var item in type.Assembly.GetTypes())
            {
                var tt = item.GetTypeInfo();
                var interfaces = tt.GetInterfaces();
                var inter = interfaces.FirstOrDefault(t => t.FullName == type.FullName);
                if (inter != null)
                {
                    ty = tt;
                    break;
                }
            }
            if (ty == null)
                throw new Exception("找不到该接口的实现类");
            var basetype = ty.BaseType.GetTypeInfo();
            var method = basetype.GetMethod("GetInstace", new[] { typeof(Type) });
#endif
            return (T)method.Invoke(null, new[] { ty });
        }
    }
}
