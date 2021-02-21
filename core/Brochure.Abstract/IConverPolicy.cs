using System;
using System.Text.Json;
namespace Brochure.Abstract
{
    public interface IConverPolicy
    {
        T2 ConverTo<T1, T2> (T1 model) where T1 : class
        where T2 : class;
    }
}