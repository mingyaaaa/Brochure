using System;

namespace Brochure.Core
{
    /// <summary>
    /// 转化时 需要忽略的属性
    /// </summary>
    [AttributeUsage (AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class IngoreAttribute : Attribute { }
}