using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;

namespace Brochure.Core.Server
{
    /// <summary>
    /// The plugin action descriptor change provider.
    /// </summary>
    public class PluginActionDescriptorChangeProvider : IActionDescriptorChangeProvider
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static PluginActionDescriptorChangeProvider Instance { get; } = new PluginActionDescriptorChangeProvider();

        /// <summary>
        /// Gets the token source.
        /// </summary>
        public CancellationTokenSource TokenSource { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether has changed.
        /// </summary>
        public bool HasChanged { get; set; }

        /// <summary>
        /// Gets the change token.
        /// </summary>
        /// <returns>An IChangeToken.</returns>
        public IChangeToken GetChangeToken()
        {
            TokenSource = new CancellationTokenSource();
            return new CancellationChangeToken(TokenSource.Token);
        }
    }
}