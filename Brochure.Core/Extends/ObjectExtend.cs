using System;
using Brochure.Core.Interfaces;
using BaseType = Brochure.Core.System.BaseType;
namespace Brochure.Core.Extends
{
    public static class ObjectExtend
    {
        /// <summary>
        /// 类型转换  
        /// 转换不成功 回返回默认值 
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="exc">异常，如果部位null 则会throw 异常</param>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns></returns>
        public static T As<T>(this object obj, Exception exc = null)
        {

            if (obj == null)
                return (T)(object)null;
            var type = typeof(T);
            var objType = obj.GetType();
            if (type == objType)
                return (T)obj;
            //如果是枚举类型 着转化为整形
            if (obj is Enum)
                obj = (int)obj;
            if (type.BaseType == typeof(Enum))
                return (T)Enum.Parse(type, obj.ToString());
            try
            {
                //如果是基本类型 使用基本类型转换
                if (type.Name == BaseType.String)
                    return (T)(object)obj.ToString();
                else if (type.Name == BaseType.Int)
                    return (T)(object)int.Parse(obj.ToString());
                else if (type.Name == BaseType.Double)
                    return (T)(object)double.Parse(obj.ToString());
                else if (type.Name == BaseType.Float)
                    return (T)(object)float.Parse(obj.ToString());
                else if (type.Name == BaseType.DateTime)
                    return (T)(object)DateTime.Parse(obj.ToString());
                //实现接口IBConverable的类使用 借口转换气
                var interfaceType = type.GetInterface("IBConverable");
                var ibConver = interfaceType as IBConverables;
                if (ibConver != null)
                    return ibConver.Conver<T>(obj);
                //如果是系统其他类型  则使用系统的转换器
                return (T)(object)Convert.ChangeType(obj, type);
            }
            catch (Exception)
            {
                if (exc != null)
                    throw exc;
                return (T)(object)default(T);
            }
        }
    }
}