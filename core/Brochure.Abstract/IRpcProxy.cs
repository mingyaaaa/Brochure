using System;
namespace Brochure.Abstract
{
    /// <summary>
    /// The rpc proxy.
    /// </summary>
    public interface IRpcProxy<T>
    {
        /// <summary>
        /// Gets the ins.
        /// </summary>
        T Ins { get; }
    }
}
