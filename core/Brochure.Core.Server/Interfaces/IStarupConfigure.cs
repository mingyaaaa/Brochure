﻿using Microsoft.AspNetCore.Builder;
using System;

namespace Brochure.Core.Server
{
    /// <summary>
    /// The starup configure.
    /// </summary>
    public interface IStarupConfigure
    {
        /// <summary>
        /// Configures the.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="builder">The builder.</param>
        void Configure(Guid key, IApplicationBuilder builder);
    }
}