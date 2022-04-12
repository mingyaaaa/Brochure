using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Brochure.Core.Server
{
    /// <summary>
    /// The middle manager.
    /// </summary>
    public interface IMiddleManager
    {
        /// <summary>
        /// Gets or sets the middle action.
        /// </summary>
        Action<Func<RequestDelegate, RequestDelegate>> MiddleAction { get; set; }

        /// <summary>
        /// Adds the middle.
        /// </summary>
        /// <param name="middleName">The middle name.</param>
        /// <param name="pluginId">The plugin id.</param>
        /// <param name="middle">The middle.</param>
        void AddMiddle(string middleName, Guid pluginId, Func<RequestDelegate, RequestDelegate> middle);

        /// <summary>
        /// Interts the middle.
        /// </summary>
        /// <param name="middleName">The middle name.</param>
        /// <param name="pluginId">The plugin id.</param>
        /// <param name="index">The index.</param>
        /// <param name="middle">The middle.</param>
        void IntertMiddle(string middleName, Guid pluginId, int index, Func<RequestDelegate, RequestDelegate> middle);

        /// <summary>
        /// Removes the plugin middle.
        /// </summary>
        /// <param name="guid">The guid.</param>
        void RemovePluginMiddle(Guid guid);

        /// <summary>
        /// Gets the middles list.
        /// </summary>
        /// <returns>A list of RequestDelegateProxies.</returns>
        IReadOnlyList<RequestDelegateProxy> GetMiddlesList();

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        void AddRange(IEnumerable<RequestDelegateProxy> proxy);

        /// <summary>
        /// Resets the.
        /// </summary>
        void Reset();
    }
}