using System.Collections;
using System.Collections.Generic;
namespace LinqDbQuery
{
    public static class ObjectTypeUtils
    {
        public static bool IsIEnumerable (object obj)
        {
            var type = obj.GetType ();
            ///如果是string 则不是集合接口
            if (obj is string)
                return false;
            return obj is IEnumerable;
        }
    }
}