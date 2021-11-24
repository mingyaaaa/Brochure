using System;
namespace Brochure.Abstract
{
    public interface IRpcProxy<T>
    {
        /// <summary>
        /// Gets the ins.
        /// </summary>
        T Ins { get; }
    }
}
