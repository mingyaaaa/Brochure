using System;

namespace Brochure.Abstract
{
    /// <summary>
    /// The plugin load action.
    /// </summary>
    public interface IPluginLoadAction
    {
        /// <summary>
        /// Invokes the.
        /// </summary>
        /// <param name="key">The key.</param>
        void Invoke (Guid key);
    }

    /// <summary>
    /// The default load action.
    /// </summary>
    public class DefaultLoadAction : IPluginLoadAction
    {
        /// <summary>
        /// Invokes the.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Invoke (Guid key) { }
    }
    /// <summary>
    /// The plugin un load action.
    /// </summary>
    public interface IPluginUnLoadAction
    {
        /// <summary>
        /// Invokes the.
        /// </summary>
        /// <param name="key">The key.</param>
        void Invoke (Guid key);
    }
    /// <summary>
    /// The default un load action.
    /// </summary>
    public class DefaultUnLoadAction : IPluginUnLoadAction
    {
        /// <summary>
        /// Invokes the.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Invoke (Guid key) { }
    }
}