using System;
using Microsoft.AspNetCore.Builder;

namespace Brochure.Core.Server
{
    public interface IStarupConfigure
    {
        void Configure (Guid key, IApplicationBuilder builder);
    }
}