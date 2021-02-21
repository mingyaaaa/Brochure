using System;
namespace Brochure.Abstract
{
    public interface IRpcProxy<T>
    {
        T Ins { get; }
    }
}
