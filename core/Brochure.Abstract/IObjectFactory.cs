namespace Brochure.Abstract
{
    /// <summary>
    /// The object factory.
    /// </summary>
    public interface IObjectFactory
    {
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Create<T>() where T : new();

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs">对象参数</param>
        /// <returns></returns>
        T Create<T>(params object[] objs) where T : class;

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        object Create(Type type, params object[] objs);

        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="type"></param>
        /// <param name="objs">The objs.</param>
        /// <returns>An object.</returns>
        object CreateByIoc(IServiceProvider serviceProvider, Type type, params object[] objs);

        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="objs">The objs.</param>
        /// <returns>A T.</returns>
        T CreateByIoc<T>(IServiceProvider service, params object[] objs);
    }
}