using System;
using System.Text.Json;
namespace Brochure.Abstract
{
    public interface IConverPolicy
    {
        T2 ConverTo<T1, T2>(T1 model) where T1 : class
        where T2 : class, new();
    }

    public interface IConverPolicy<T1, T2> where T1 : class
        where T2 : class, new()
    {
        T2 ConverTo(T1 model);
    }

    public interface IConverPolicy<T> where T : class
    {
        T ConverTo(T obj = null);
    }
}