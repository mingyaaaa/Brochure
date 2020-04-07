using System;

namespace LinqDbQuery
{
    /// <summary>
    /// 转化时 需要忽略的属性
    /// </summary>
    [AttributeUsage (AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal sealed class IngoreAttribute : Attribute { }
}