using System.Reflection;

namespace Brochure.Abstract.Utils
{
    /// <summary>
    /// The reflector util.
    /// </summary>
    public interface IReflectorUtil
    {
        /// <summary>
        /// 根据接口创建创建指定的对象
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="type">创建的类型</param>
        /// <returns></returns>
        IEnumerable<object> GetObjectOfBase(Assembly assembly, Type type);

        /// <summary>
        /// 根据接口创建创建指定的对象
        /// </summary>
        /// <typeparam name="T">创建的类型</typeparam>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        IEnumerable<T> GetObjectOfBase<T>(Assembly assembly);

        /// <summary>
        /// 根据接口获取指定类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<Type> GetTypeOfBase(Assembly assembly, Type type);

        /// <summary>
        /// 根据接口获取指定类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<Type> GetTypeOfBase<T>(Assembly assembly);

        /// <summary>
        /// 获取直接继承父类
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<object> GetObjectOfAbsoluteBase(Assembly assembly, Type type);

        /// <summary>
        /// 获取直接继承父类
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<T> GetObjectOfAbsoluteBase<T>(Assembly assembly);

        /// <summary>
        /// 获取直接继承父类类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<Type> GetTypeOfAbsoluteBase(Assembly assembly, Type type);

        /// <summary>
        /// 获取直接继承父类类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<Type> GetTypeOfAbsoluteBase<T>(Assembly assembly);

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parms"></param>
        /// <returns></returns>
        T CreateInstance<T>(params object[] parms) where T : class;

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        object CreateInstance(Type type, params object[] parms);

        /// <summary>
        /// 获取属性设置方法
        /// </summary>
        /// <param name="propertyName"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        Action<T1, T2> GetSetPropertyValueFun<T1, T2>(string propertyName);

        /// <summary>
        /// 获取属性设置方法
        /// </summary>
        /// <param name="valueClass"></param>
        /// <param name="propertyName"></param>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        Action<T1, object> GetSetPropertyValueFun<T1>(Type valueClass, string propertyName);

        /// <summary>
        /// 获取属性值方法
        /// </summary>
        /// <param name="propertyName"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        Func<T1, T2> GetPropertyValueFun<T1, T2>(string propertyName);

        /// <summary>
        /// 获取属性值方法
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        Func<T1, object> GetPropertyValueFun<T1>(string propertyName);

        /// <summary>
        /// Gets the property value fun.
        /// </summary>
        /// <param name="classType">The class type.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>A Func.</returns>
        public Func<object, object> GetPropertyValueFun(Type classType, string propertyName);
    }
}