using System;
using Microsoft.AspNetCore.Builder;

namespace Brochure.Server.Main.Abstract.Interfaces
{
    public interface IStarupConfigure
    {
        void Configure (IApplicationBuilder builder);
    }
}