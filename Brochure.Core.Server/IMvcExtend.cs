using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Brochure.Core.Server
{
    public static class IMvcExtend
    {
        /// <summary>
        /// 加载插件程序集
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <param name="assembly"></param>
        public static void LoadPluginAssembyl(this IMvcBuilder mvcBuilder, Assembly assembly)
        {
            if (mvcBuilder == null)
            {
                throw new ArgumentException("参数异常");
            }
            mvcBuilder.AddApplicationPart(assembly);
        }
    }
}
