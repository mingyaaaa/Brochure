using System;
using System.Text.Json;
namespace Brochure.Abstract
{
    /// <summary>
    /// Model和Entiry之间转换策略接口
    /// </summary>
    public interface IConverPolicy<T1, T2> where T1 : class
    where T2 : class
    {
        T2 ConverTo (T1 model);

    }
}