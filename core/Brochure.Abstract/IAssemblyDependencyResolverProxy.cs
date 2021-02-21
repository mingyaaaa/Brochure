using System;
using System.Reflection;

namespace Brochure.Abstract
{
    /// <summary>
    /// 程序集依赖加载接口
    /// </summary>
    public interface IAssemblyDependencyResolverProxy
    {
        /// <summary>
        /// 解析程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        string ResolveAssemblyToPath(AssemblyName assemblyName);


        string ResolveUnmanagedDllToPath(string name);
    }
}
